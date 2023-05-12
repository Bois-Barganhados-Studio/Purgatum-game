using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Leon Jr
 */
public class ProceduralMapBuilder : MonoBehaviour
{
    private static readonly List<int> MATRIX_LEFT_ELEMENTS = new List<int> { 0, 4, 6, 12 };
    private static readonly List<int> MATRIX_RIGHT_ELEMENTS = new List<int> { 3, 7, 11, 15 };
    private RenderLevel levelRenderer = null;
    private const string DIVIDER = "_";
    private const string FOLDER = "Maps/";
    private static readonly string[] styles = { "STONE" };
    private static List<string> stylesIni = new List<string>();
    private BluePrintReader bpReader = null;
    public int tileNumber;
    public int[] bp;
    private int groundCounter = 1, wallCounter = 1;
    public int roomStyle = 0, randFactor = 6, numOfRooms = 5;
    private static readonly string GROUND = "GROUND";
    private static readonly string WALL = "WALL";
    private int GROUNDS_SZ = 0, WALLS_SZ = 0;

    public ProceduralMapBuilder()
    {
        Debug.Log("INICIANDO GERA��O DE MUNDO");
        bpReader = new BluePrintReader();
    }

    /**
     * Ler iniciadores para os tilemaps
     */
    void ReadIni(string path)
    {
        TextAsset iniFile = Resources.Load<TextAsset>(FOLDER + path);
        string[] lines = iniFile.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimEnd('\r', '\n');
            if (lines[i].Contains("GROUND"))
            {
                GROUNDS_SZ++;
            }
            else if (lines[i].Contains("WALL"))
            {
                WALLS_SZ++;
            }
        }
        stylesIni.AddRange(lines);
    }

    void Start()
    {
        //criar func so para isso e gerar bps
        bp = new int[numOfRooms];
        bp[0] = 2;
        bp[1] = 0;
        bp[2] = 1;
        bp[3] = 3;
        bp[4] = 4;
        if (stylesIni.Count == 0)
        {
            foreach (string style in styles)
            {
                ReadIni(style + "/" + style.ToLower());
            }
        }
        if (BuildTerrain(roomStyle, randFactor, numOfRooms, bp))
        {
            Debug.Log("MAPA GERADO COM SUCESSO: " + bp);
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Iniciar o novo nivel e limpar renderizador
     */
    void NewLevel()
    {
        bp = new int[numOfRooms];
        bp[0] = 0;
        bp[1] = 4;
        bp[2] = 3;
        bp[3] = 2;
        bp[4] = 1;
        levelRenderer.UnloadMemory();
        levelRenderer.ClearGameObject();
        if (BuildTerrain(roomStyle, randFactor, numOfRooms, bp))
        {
            Debug.Log("MAPA GERADO COM SUCESSO");
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Inicia a constru��o de um n�vel da torre
     * @args roomStyle estilo da sala (STONE, GRASS, FIRE etc)
     * @args randFactor fator de aleatoriedade da constru��o do mapa (SEED)
     * @args numRooms n�mero de salas que v�o ser geradas nesse nivel
     */
    bool BuildTerrain(int roomStyle, int randFactor, int numRooms, int[] blueprint)
    {
        bool status = false;
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        try
        {
            int[] matrixIndexes = new int[numRooms];
            matrixIndexes = GenerateMapIndexes(matrixIndexes.Length);
            //usar randFactor para definir posi��es no mapa dos elementos
            levelRenderer = new RenderLevel();
            List<string> chunkPaths = new List<string>();
            List<Vector3> chunkPositions = new List<Vector3>();
            bpReader.SetNumOfRooms(numRooms);
            bpReader.defineBp(blueprint);
            //List<Room> structures = bpReader.RoomsLoaded();
            Map map = new Map(bpReader.RoomsLoaded(), matrixIndexes);
            //alterar todo o metodo para usar o objeto de mapa
            //salvar I na classe Room para isso definir os I de matrix aqui usando algum metodo qualquer!
            //recalcular as posi��es baseado no I da matriz dos elementos por exemplo (xI*0,yI*10,20)
            foreach (Room room in map.GetRooms())
            {
                //Debug.Log("mmsz " + room.SizeOf());
                for (int i = 0; i < room.SizeOf(); i++)
                {
                    string structure = room.GetBlock(i);
                    if (structure != null)
                    {
                        Vector3 position = room.GetPosition(i);
                        string path = "";
                        if (structure.Equals("SPAWN"))
                        {
                            if (spawnPoint.x == 0 && spawnPoint.y == 0)
                            {
                                path = structure + DIVIDER + styles[roomStyle];
                                spawnPoint = position;
                            }
                            else
                            {
                                //DEFAULT GROUND
                                path = GROUND + DIVIDER + styles[roomStyle] + DIVIDER + NextGroundEnumerate();
                            }
                        }
                        else
                        {
                            path = structure + DIVIDER + styles[roomStyle];
                            if (structure.Equals(GROUND))
                            {
                                path += DIVIDER + NextGroundEnumerate();
                            }
                            else if (structure.Equals(WALL))
                            {
                                path += DIVIDER + NextWallEnumerate();
                            }
                        }
                        Debug.Log(path);
                        if (stylesIni.Contains(path))
                        {
                            chunkPaths.Add(FOLDER + styles[roomStyle] + "/" + path);
                            chunkPositions.Add(position);
                        }
                    }
                }
            }
            levelRenderer.AddChunks(chunkPaths, chunkPositions);
            GameObject.Find("Player").transform.position = spawnPoint;
            GameObject.Find("Enemy").transform.position = spawnPoint + new Vector3(1.5f, 1.0f, 0);
            GameObject.Find("Pathfinding").transform.position = spawnPoint + new Vector3(1.5f, 1.0f, 0);
            levelRenderer.RenderElements();
            status = true;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        return status;
    }

    int NextGroundEnumerate()
    {
        if (groundCounter == randFactor)
        {
            groundCounter++;
            int gd = Random.Range(1, GROUNDS_SZ);
            return (gd == 8 || gd == 10) ? 1 : gd;
        }
        if (groundCounter < GROUNDS_SZ)
        {
            groundCounter++;
        }
        else
        {
            groundCounter = 1;
        }
        return 1;
    }

    int NextWallEnumerate()
    {
        if (wallCounter == randFactor)
        {
            wallCounter = 1;
            return Random.Range(wallCounter, WALLS_SZ);
        }
        if (wallCounter < WALLS_SZ)
        {
            wallCounter++;
        }
        else
        {
            wallCounter = 1;
        }
        return wallCounter;
    }

    /**
     * Fun��o de mapeamento para gerar posi��es das salas em uma grade
     * de mapa para 4x4 matricial
     */
    int[] GenerateMapIndexes(int numOfRooms)
    {
        int[] matrix = new int[numOfRooms];
        int origin = UnityEngine.Random.Range(0, 15);
        matrix[0] = origin;
        List<int> conj = new List<int>();
        List<int> escolhas = new List<int> { 0, 1, 2, 3 };
        conj.Add(origin);
        int index = 1, selected = 0;
        int conjElement = conj[0], op = 0;
        do
        {
            op = escolhas[Random.Range(0, escolhas.Count)];
            // Debug.Log("op: " + op);
            if (op == 0)
            {
                if (!MATRIX_LEFT_ELEMENTS.Contains(op))
                {
                    selected = conjElement - 1;
                }
                else
                {
                    escolhas.Remove(op);
                }
            }
            else if (op == 1)
            {
                if (!MATRIX_RIGHT_ELEMENTS.Contains(op))
                {
                    selected = conjElement + 1;
                }
                else
                {
                    escolhas.Remove(op);
                }
            }
            else if (op == 2)
            {
                selected = conjElement + 4;
            }
            else
            {
                selected = conjElement - 4;
            }
            if (index != numOfRooms && (selected >= 0 && selected <= 15) && !System.Array.Exists(matrix, element => element == selected))
            {
                matrix[index] = selected;
                conj.Add(selected);
                conjElement = conj[Random.Range(0, conj.Count)];
                if (conjElement == matrix[index] && escolhas.Count == 1)
                {
                    conjElement = selected;
                }
                else
                {
                    escolhas = new List<int> { 0, 1, 2, 3 };
                }
                index++;
            }
            else
            {
                if (escolhas.Count > 0)
                {
                    escolhas.Remove(op);
                    //   Debug.Log("removed: " + escolhas.Count + " - " + op);
                    if (escolhas.Count == 0)
                    {
                        conj.Remove(conjElement);
                        if (conj.Count > 0)
                        {
                            conjElement = conj[Random.Range(0, conj.Count)];
                            escolhas = new List<int> { 0, 1, 2, 3 };
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        } while (index < numOfRooms);
        return matrix;
    }
}
