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
    //pametros públicos para uso do script (valores default estão definidos)
    public int[] blueprints;
    public int roomStyle = 0, randFactor = 6, numOfRooms = 5, origin = 0;
    //parametros internos do sistema de criação do mapa do jogo
    private RenderLevel levelRenderer = null;
    private BluePrintReader blueprintsReader = null;
    private static List<string> stylesIni = new List<string>();
    private List<(int, int)> pathRooms = new List<(int, int)>();
    private GameObject player = null;
    private GameObject targetObject = null;
    //constantes
    private static readonly List<int> MATRIX_LEFT_ELEMENTS = new List<int> { 0, 4, 8, 12 };
    private static readonly List<int> MATRIX_RIGHT_ELEMENTS = new List<int> { 3, 7, 11, 15 };
    private const string DIVIDER = "_";
    private const float G_NODE_SIZE = 0.4f;
    private const string FOLDER = "Maps/";
    private const int DOORS_LIMIT = 10;
    private static readonly string[] styles = { "STONE" };
    private static readonly string GROUND = "GROUND", WALL = "WALL", DOOR = "GROUND";
    //utils
    private int groundCounter = 1, wallCounter = 1;
    private int groundsSize = 0, wallsSize = 0;

    public ProceduralMapBuilder()
    {
        Debug.Log("INICIANDO MUNDO PROCEDURAL...");
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
                groundsSize++;
            }
            else if (lines[i].Contains(WALL))
            {
                wallsSize++;
            }
        }
        stylesIni.AddRange(lines);
    }

    /**
     * Iniciar a build do mapa na primeira run
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
    private void CenterGraph(Vector3 spawn, int width, int height)
    {
        targetObject.transform.position = spawn;
        AstarPath ast = targetObject.GetComponent<AstarPath>();
        ast.data.gridGraph.center = spawn;
        ast.data.gridGraph.width = width;
        ast.data.gridGraph.depth = height;
        ast.data.gridGraph.SetDimensions(width, height, G_NODE_SIZE); //setDimensions
        ast.data.gridGraph.Scan();
    }

    async void Start()
    {
        await BuildMap();
        CenterGraph(new Vector3((levelRenderer.x_max + levelRenderer.x_min) / 2, (levelRenderer.y_max + levelRenderer.y_min) / 2, 0),
                       (int)(2.6 * (Mathf.Abs(levelRenderer.x_max) + Mathf.Abs(levelRenderer.x_min))),
                       (int)(2.2 * (Mathf.Abs(levelRenderer.y_max) + Mathf.Abs(levelRenderer.y_min))));
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
    public async void NewLevel()
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
        CenterGraph(new Vector3((levelRenderer.x_max + levelRenderer.x_min) / 2, (levelRenderer.y_max + levelRenderer.y_min) / 2, 0),
                       (int)(2.6 * (Mathf.Abs(levelRenderer.x_max) + Mathf.Abs(levelRenderer.x_min))),
                       (int)(2.2 * (Mathf.Abs(levelRenderer.y_max) + Mathf.Abs(levelRenderer.y_min))));
    }

    /**
     * Inicia a constru��o de um n�vel da torre
     * @args roomStyle estilo da sala (STONE, GRASS, FIRE etc)
     * @args randFactor fator de aleatoriedade da constru��o do mapa (SEED)
     * @args numOfRooms n�mero de salas que v�o ser geradas nesse nivel
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
            GenerateGlobalDoors(map);
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
            //remover isso depois (apenas para testes)
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

    /**
     * Proximo chão na escolha da sala
     * @return int numero da sala dentro da folder
     */
    int NextGroundEnumerate()
    {
        //criar modo de default ground para ter varios chãos padrões
        if (groundCounter == randFactor)
        {
            groundCounter = 0;
            int gd = Random.Range(1, groundsSize);
            //condição para remover tiles bugados (remover depois)
            return (gd == 8 || gd == 10) ? 1 : gd;
        }
        if (groundCounter < groundsSize)
        {
            groundCounter++;
        }
        else
        {
            groundCounter = 1;
        }
        return groundCounter == 0 ? groundCounter : 1;
    }

    /**
     * Proxima parede na escolha da sala
     * @return int numero da sala dentro da folder
     */
    int NextWallEnumerate()
    {
        if (wallCounter == randFactor)
        {
            wallCounter = 1;
            return Random.Range(wallCounter, wallsSize);
        }
        if (wallCounter < wallsSize)
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
     * Função de mapeamento para gerar posi��es das salas em uma grade
     * de mapa para 4x4 matricial
     */
    Task<int[]> GenerateMapIndexes()
    {
        int[] matrix = new int[numOfRooms];
        int index = 1, selected = 0;
        List<int> conj = new List<int>();
        List<int> escolhas = new List<int> { 0, 1, 2, 3 };
        bool ignore = false;
        pathRooms.Clear();
        origin = (origin != 0 ? origin : UnityEngine.Random.Range(0, 15));
        matrix[0] = origin;
        conj.Add(origin);
        int conjElement = conj[0], op = 0, realSize = 1;
        do
        {
            op = escolhas[Random.Range(0, escolhas.Count)];
            ignore = false;
            if (op == 0)
            {
                if (!MATRIX_LEFT_ELEMENTS.Contains(conjElement))
                {
                    selected = conjElement - 1;
                }
                else
                {
                    ignore = true;
                }
            }
            else if (op == 1)
            {
                if (!MATRIX_RIGHT_ELEMENTS.Contains(conjElement))
                {
                    selected = conjElement + 1;
                }
                else
                {
                    ignore = true;
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
            if (!ignore && index != numOfRooms && (selected >= 0 && selected <= 15) && !System.Array.Exists(matrix, element => element == selected))
            {
                matrix[index] = selected;
                conj.Add(selected);
                pathRooms.Add((conjElement, selected));
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
    /*
     * Gerar posição das portas nas salas globalmente
     * intersections: (RL - 1, LR - 2, DU - 3, UD - 4)
     */
    void GenerateGlobalDoors(Map map)
    {
        int intersection = 0;
        int diff = 0;
        List<int> indexes = new List<int>();
        List<(int, int)> targets = new List<(int, int)>();
        foreach ((int, int) path in pathRooms)
        {
            if (!indexes.Contains(path.Item1))
            {
                indexes.Add(path.Item1);
            }
            if (!indexes.Contains(path.Item2))
            {
                indexes.Add(path.Item2);
            }
        }
        foreach ((int, int) path in pathRooms)
        {
            if (path.Item1 < path.Item2)
            {
                diff = path.Item2 - path.Item1;
                if (diff == 1)
                {
                    intersection = 1;
                }
                else if (diff == -1)
                {
                    intersection = 2;
                }
                else if (diff == 4)
                {
                    intersection = 3;
                }
                else if (diff == -4)
                {
                    intersection = 4;
                }
            }
            else
            {
                diff = path.Item1 - path.Item2;
                if (diff == 1)
                {
                    intersection = 2;
                }
                else if (diff == -1)
                {
                    intersection = 1;
                }
                else if (diff == 4)
                {
                    intersection = 4;
                }
                else if (diff == -4)
                {
                    intersection = 3;
                }
            }
            Room room1 = map.GetRoom(indexes.IndexOf(path.Item1));
            Room room2 = map.GetRoom(indexes.IndexOf(path.Item2));
            switch (intersection)
            {
                case 1:
                    targets = GetTargetDoors(room1.GetMaxRight(), room2.GetMaxLeft());
                    break;
                case 2:
                    targets = GetTargetDoors(room1.GetMaxLeft(), room2.GetMaxRight());
                    break;
                case 3:
                    targets = GetTargetDoors(room1.GetMaxDown(), room2.GetMaxUp());
                    break;
                case 4:
                    targets = GetTargetDoors(room1.GetMaxUp(), room2.GetMaxDown());
                    break;
                default:
                    break;
            }
            if (targets.Count >= DOORS_LIMIT)
            {
                int divider = targets.Count / 3;
                for (int i = divider; i < targets.Count - divider; i++)
                {
                    (int, int) block = targets[i];
                    room1.UpdateBlock(DOOR, block.Item1);
                    room2.UpdateBlock(DOOR, block.Item2);
                }
            }
            else
            {
                foreach ((int, int) block in targets)
                {
                    room1.UpdateBlock(DOOR, block.Item1);
                    room2.UpdateBlock(DOOR, block.Item2);
                }
            }

        }
    }

    /**
     * Definir portas usando regras e listas de MAX definidos na classe ROOM
     * Encontrar posições semelhantes para remover walls e adicionar grounds
     */
    List<(int, int)> GetTargetDoors(List<(int, int)> max1, List<(int, int)> max2)
    {
        List<(int, int)> targets = new List<(int, int)>();
        foreach ((int, int) target in max1)
        {
            foreach ((int, int) target2 in max2)
            {
                if (target.Item2 == target2.Item2)
                {
                    targets.Add((target.Item1, target2.Item1));
                }

            }
        }
        return targets;
    }

}
