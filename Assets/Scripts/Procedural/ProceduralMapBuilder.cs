using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Pathfinding;

/**
 * @author Leon Jr
 */
public class ProceduralMapBuilder : MonoBehaviour
{
    private static readonly List<int> MATRIX_LEFT_ELEMENTS = new List<int> { 0, 4, 8, 12 };
    private static readonly List<int> MATRIX_RIGHT_ELEMENTS = new List<int> { 3, 7, 11, 15 };
    private RenderLevel levelRenderer = null;
    private const string DIVIDER = "_";
    private const string FOLDER = "Maps/";
    private static readonly string[] styles = { "STONE" };
    private static List<string> stylesIni = new List<string>();
    private BluePrintReader blueprintsReader = null;
    private int tileNumber;
    public int[] blueprints;
    private int groundCounter = 1, wallCounter = 1;
    public int roomStyle = 0, randFactor = 6, numOfRooms = 5;
    private static readonly string GROUND = "GROUND";
    private static readonly string WALL = "WALL";
    private int GROUNDS_SZ = 0, WALLS_SZ = 0;
    public int origin = 0;
    private GameObject player = null;
    private GameObject targetObject = null;

    public ProceduralMapBuilder()
    {
        Debug.Log("INICIANDO GERAÇÂO DE MUNDO");
        blueprintsReader = new BluePrintReader();
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
            if (lines[i].Contains(GROUND))
            {
                GROUNDS_SZ++;
            }
            else if (lines[i].Contains(WALL))
            {
                WALLS_SZ++;
            }
        }
        stylesIni.AddRange(lines);
    }
    
    /**
     * Iniciar a construção do mapa na primeira run
     */
    private async Task BuildMap()
    {
        player = GameObject.Find("Player");
        targetObject = GameObject.Find("Pathfinding");
        if (stylesIni.Count == 0)
        {
            foreach (string style in styles)
            {
                ReadIni(style + "/" + style.ToLower());
            }
        }
        if (await BuildTerrainAsync())
        {
            Debug.Log("MAPA GERADO COM SUCESSO");
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Centralizar o grafo dos inimigos para o meio do mapa
     * Define os bounds do grafo
     */
    void CenterGraph(Vector3 spawn, int width, int height)
    {
        Debug.Log(spawn);
        Debug.Log(width);
        Debug.Log(height);
        targetObject.transform.position = spawn;
        AstarPath ast = targetObject.GetComponent<AstarPath>();
        ast.data.gridGraph.center = spawn;
        ast.data.gridGraph.width = width;
        ast.data.gridGraph.depth = height;
        //ast.data.gridGraph.SetDimensions();
        ast.data.gridGraph.UpdateSizeFromWidthDepth(); //setDimensions
        ast.data.gridGraph.Scan();
    }

    async void Start()
    {

        await BuildMap();
        CenterGraph(new Vector3((levelRenderer.x_max + levelRenderer.x_min) / 2, (levelRenderer.y_max + levelRenderer.y_min) / 2, 0),
                 (int)(2.5 * (Mathf.Abs(levelRenderer.x_max) + Mathf.Abs(levelRenderer.x_min))), (int)(3 * (Mathf.Abs(levelRenderer.y_max) + Mathf.Abs(levelRenderer.y_min))));
    }

    /**
     * Codigo para escolher automaticamente blueprints aleatorias para o mapa
     */
    int[] ChooseBlueprints()
    {
        int[] bps = new int[numOfRooms];
        for (int i = 0; i < numOfRooms; i++)
        {
            bps[i] = UnityEngine.Random.Range(0, BluePrintReader.BLUEPRINTS_TOTAL - 1);
        }
        return bps;
    }

    /**
     * Iniciar o novo nivel e limpar renderizador
     */
    async void NewLevel()
    {
        levelRenderer.UnloadMemory();
        levelRenderer.ClearGameObject();
        if (await BuildTerrainAsync())
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
     * @args numOfRooms número de salas que vão ser geradas nesse nivel
     */
    async Task<bool> BuildTerrainAsync()
    {
        bool status = false;
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        try
        {
            int[] matrixIndexes = await GenerateMapIndexes();
            if (blueprints == null || blueprints.Length == 0)
            {
                blueprints = ChooseBlueprints();
            }
            levelRenderer = new RenderLevel();
            List<string> chunkPaths = new List<string>();
            List<Vector3> chunkPositions = new List<Vector3>();
            blueprintsReader.SetNumOfRooms(numOfRooms);
            blueprintsReader.defineBp(blueprints);
            Map map = new Map(blueprintsReader.RoomsLoaded(), matrixIndexes);
            foreach (Room room in map.GetRooms())
            {
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
                        if (stylesIni.Contains(path))
                        {
                            chunkPaths.Add(FOLDER + styles[roomStyle] + "/" + path);
                            chunkPositions.Add(position);
                        }
                    }
                }
            }
            levelRenderer.AddChunks(chunkPaths, chunkPositions);
            player.transform.position = spawnPoint;
            GameObject.Find("Enemy").transform.position = spawnPoint + new Vector3(1.5f, 1.0f, 0);
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
            groundCounter = 0;
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
        return groundCounter == 0 ? groundCounter : 1;
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
     * Função de mapeamento para gerar posições das salas em uma grade
     * de mapa para 4x4 matricial
     */
    Task<int[]> GenerateMapIndexes()
    {
        int[] matrix = new int[numOfRooms];
        origin = (origin != 0 ? origin : UnityEngine.Random.Range(0, 15));
        matrix[0] = origin;
        List<int> conj = new List<int>();
        List<int> escolhas = new List<int> { 0, 1, 2, 3 };
        conj.Add(origin);
        int index = 1, selected = 0;
        int conjElement = conj[0], op = 0, realSize = 1;
        do
        {
            op = escolhas[Random.Range(0, escolhas.Count)];
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
                realSize++;
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
                    if (escolhas.Count == 0)
                    {
                        conj.Remove(conjElement);
                        if (conj.Count > 0)
                        {
                            conjElement = conj[Random.Range(0, conj.Count)];
                            escolhas = new List<int> { 0, 1, 2, 3 };
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
        numOfRooms = realSize;
        return Task.FromResult(matrix);
    }
}
