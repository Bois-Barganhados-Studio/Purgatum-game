using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * @author Leon Jr
 */
public class ProceduralMapBuilder : MonoBehaviour
{

    private RenderLevel levelRenderer = null;
    private const string DIVIDER = "_";
    private const string FOLDER = "Maps/";
    private static readonly string[] styles = { "STONE" };
    private static List<string> stylesIni = new List<string>();
    private BluePrintReader bpReader = null;
    public int tileNumber;
    public int[] bp;
    public int numOfRooms = 5;
    public int roomStyle = 0;

    public ProceduralMapBuilder()
    {
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
        }
        stylesIni.AddRange(lines);
    }

    void Start()
    {
        bp = new int[numOfRooms];
        bp[0] = 0;
        bp[1] = 4;
        bp[2] = 4;
        bp[3] = 0;
        bp[4] = 4;
        Debug.Log(bp.ToString());
        Debug.Log(bpReader);
        //bp[0] = 3;
        if (stylesIni.Count == 0)
        {
            foreach (string style in styles)
            {
                ReadIni(style + "/" + style.ToLower());
            }
        }
        if (BuildTerrain(roomStyle, 0, numOfRooms, bp, UnityEngine.Random.Range(1, 4)))
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
        bp[0] = 1;
        bp[1] = 2;
        levelRenderer.UnloadMemory();
        levelRenderer.ClearGameObject();
        if (BuildTerrain(roomStyle, 0, numOfRooms, bp, UnityEngine.Random.Range(1, 4)))
        {
            Debug.Log("MAPA GERADO COM SUCESSO");
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Inicia a construção de um nível da torre
     * @args roomStyle estilo da sala (STONE, GRASS, FIRE etc)
     * @args randFactor fator de aleatoriedade da construção do mapa (SEED)
     * @args numRooms número de salas que vão ser geradas nesse nivel
     * @args folderNum qual numeração na pasta de prefabs escolher para a blueprint
     */
    bool BuildTerrain(int roomStyle, int randFactor, int numRooms, int[] blueprint, int folderNum)
    {
        bool status = false;
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        try
        {
            int[] matrixIndexes = new int[numRooms];
            matrixIndexes = GenerateMapIndexes(matrixIndexes.Length);
            //usar randFactor para definir posições no mapa dos elementos
            levelRenderer = new RenderLevel();
            List<string> chunkPaths = new List<string>();
            List<Vector3> chunkPositions = new List<Vector3>();
            Debug.Log(bpReader);
            bpReader.SetNumOfRooms(numRooms);
            bpReader.defineBp(blueprint);
            //List<Room> structures = bpReader.RoomsLoaded();
            Map map = new Map(bpReader.RoomsLoaded(), matrixIndexes);
            //alterar todo o metodo para usar o objeto de mapa
            //salvar I na classe Room para isso definir os I de matrix aqui usando algum metodo qualquer!
            //recalcular as posições baseado no I da matriz dos elementos por exemplo (xI*0,yI*10,20)
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
                        folderNum = folderNum < 3 ? folderNum + 1 : 1;
                        if (structure.Equals("SPAWN"))
                        {
                            path = structure + DIVIDER + styles[roomStyle];
                            spawnPoint = position;
                        }
                        else
                        {
                            path = structure + DIVIDER + styles[roomStyle] + DIVIDER + folderNum;
                        }
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
            levelRenderer.RenderElements();
            status = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return status;
    }

    /**
     * Função de mapeamento para gerar posições das salas em uma grade
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
            op = escolhas[UnityEngine.Random.Range(0, escolhas.Count - 1)];
            selected = op == 0 ? conjElement - 1 : op == 1 ? conjElement + 1 : op == 2 ? conjElement + 4 : conjElement - 4;
            if ((selected >= 0 && selected <= 15) && !Array.Exists(matrix, element => element == selected))
            {
                matrix[index] = selected;
                conj.Add(selected);
                conjElement = conj[UnityEngine.Random.Range(0, conj.Count - 1)];
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
                }
                else
                {
                    break;
                }
            }
        } while (index < numOfRooms);
        conj.Clear();
        escolhas.Clear();
        Debug.Log("MATRIX CREATED " + matrix.Length);
        return matrix;
    }
}
