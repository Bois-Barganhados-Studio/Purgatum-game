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
    #region Variaveis publicas
    public int[] blueprints;
    public int roomStyle = 0, randFactor = 6, numOfRooms = 5, origin = -1;
    [SerializeField] public bool debugWorld = false, disableVariation = false;
    #endregion

    #region Variaveis de configuracao
    private static RenderLevel levelRenderer = null;
    private BluePrintReader blueprintsReader = null;
    private static List<string> stylesIni = new List<string>();
    private List<(int, int)> pathRooms = new List<(int, int)>();
    private GameObject player = null;
    private GameObject targetObject = null;
    //constantes
    private static readonly List<int> MATRIX_LEFT_ELEMENTS = new List<int> { 0, 4, 8, 12 };
    private static readonly List<int> MATRIX_RIGHT_ELEMENTS = new List<int> { 3, 7, 11, 15 };
    private const string DIVIDER = "_";
    private const float G_NODE_SIZE = 0.32f;
    private const string FOLDER = "Maps/";
    private const int DOORS_LIMIT = 10, DELAY_SPAWN = 18, SPAWN_CHANCE = 2;
    private static readonly string[] styles = { "STONE", "FIRE", "GRASS" };
    private static readonly string SPAWN = "SPAWN", DESPAWN = "DESPAWN", GROUND = "GROUND", WALL = "WALL", PLAIN = "PLAIN_GROUND";
    private static readonly string[] DOORS = new string[8] { "DOOR_LEFT_BOTH", "DOOR_LEFT_FIRST", "DOOR_LEFT_LAST", "DOOR_LEFT_MIDDLE", "DOOR_RIGHT_BOTH", "DOOR_RIGHT_FIRST", "DOOR_RIGHT_LAST", "DOOR_RIGHT_MIDDLE" };
    private static readonly string[] wallsName = { "SE", "NE", "SW", "NW", "S", "E", "W", "N" };
    private int groundCounter = 1;
    private int groundsSize = 0, plainsSize = 0;
    private int[] wallsSize = new int[8];
    #endregion

    #region Funcoes de configuracao
    public ProceduralMapBuilder()
    {
        Debug.Log("Procedural Map Builder Started (Geração de mapas procedurais)");
        blueprintsReader = new BluePrintReader();
    }

    async void Awake()
    {
        await BootFields();
    }

    /**
    * Iniciar o novo nivel e limpar renderizador
    */
    public async Task<bool> NewLevel(bool isBooting = true)
    {
        bool status = true;
        if (!isBooting)
        {
            levelRenderer.UnloadMemory();
            levelRenderer.ClearGameObject();
        }
        Debug.Log("NEW LEVEL STARTED");
        ReadTilesRegistred(styles[roomStyle] + "/" + styles[roomStyle].ToLower());
        if (await BuildTerrainAsync())
        {
            Debug.Log("MAPA GERADO COM SUCESSO");
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
            status = false;
        }
        CenterGraph(new Vector3((levelRenderer.x_max + levelRenderer.x_min) / 2, (levelRenderer.y_max + levelRenderer.y_min) / 2, 0),
                       (int)(2.6 * (Mathf.Abs(levelRenderer.x_max) + Mathf.Abs(levelRenderer.x_min))),
                       (int)(2.6 * (Mathf.Abs(levelRenderer.y_max) + Mathf.Abs(levelRenderer.y_min))));
        return status;
    }

    /**
     * Iniciar a build do mapa na primeira run
     */
    private async Task BootFields()
    {
        player = GameObject.Find("Player");
        targetObject = GameObject.Find("Pathfinding");
        levelRenderer = new RenderLevel();
        if (debugWorld)
        {
            ReadTilesRegistred(styles[roomStyle] + "/" + styles[roomStyle].ToLower());
            Debug.Log("DEBUG STARTED");
            if (await BuildTerrainAsync())
            {
                Debug.Log("DEBUG DE MAPA GERADO COM SUCESSO");
            }
            else
            {
                Debug.Log("ERRO AO GERAR MAPA DO JOGO");
            }
            CenterGraph(new Vector3((levelRenderer.x_max + levelRenderer.x_min) / 2, (levelRenderer.y_max + levelRenderer.y_min) / 2, 0),
                          (int)(2.6 * (Mathf.Abs(levelRenderer.x_max) + Mathf.Abs(levelRenderer.x_min))),
                          (int)(2.6 * (Mathf.Abs(levelRenderer.y_max) + Mathf.Abs(levelRenderer.y_min))));
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
        ast.data.gridGraph.SetDimensions(width, height, G_NODE_SIZE);
        ast.data.gridGraph.Scan();
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
     * Novos parametros para o novo nivel do jogo
     * @param levelData dados do nivel advindos da IA
     */
    public void SetLevelData(LevelData levelData)
    {
        this.numOfRooms = levelData.GetNumOfRooms();
        this.roomStyle = levelData.GetRoomStyle();
        this.origin = levelData.GetOrigin();
        this.randFactor = levelData.GetRandFactor();
        this.blueprints = levelData.GetBlueprints();
    }

    #endregion

    #region Build do mapa procedural
    /**
     * Inicia a construção de um nível da torre
     * @args roomStyle estilo da sala (STONE, GRASS, FIRE etc)
     * @args randFactor fator de aleatoriedade da construção do mapa (SEED)
     * @args numOfRooms numero de salas que vão ser geradas nesse nivel
     */
    async Task<bool> BuildTerrainAsync()
    {
        bool status = false, createDespawn = false, isSpawnPoint = false;
        string path = "";
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        int roomsCounter = 0;
        try
        {
            int[] matrixIndexes = await GenerateMapIndexes();
            if (blueprints == null || blueprints.Length == 0)
            {
                blueprints = ChooseBlueprints();
            }
            Dictionary<int, List<int>> spawnsPerRoom = new Dictionary<int, List<int>>();
            List<string> chunkPaths = new List<string>();
            List<Vector3> chunkPositions = new List<Vector3>();
            List<int> spawnPoints = new List<int>(), chestPoints = new List<int>();
            blueprintsReader.SetNumOfRooms(numOfRooms);
            blueprintsReader.DefineBp(blueprints);
            Map map = new Map(blueprintsReader.RoomsLoaded(), matrixIndexes);
            GenerateGlobalDoors(map);
            int numberOfChests = Random.Range(1, numOfRooms);
            foreach (Room room in map.GetRooms())
            {
                bool haveChest = false, chested = false;
                if (roomsCounter == map.GetSizeOfRooms() - 1)
                {
                    createDespawn = true;
                }
                if (numberOfChests > 0)
                {
                    numberOfChests--;
                    haveChest = true;
                }
                int numberOfSpawns = Random.Range(Room.MIN_SPAWNS, Room.MAX_SPAWNS), delaySpawn = DELAY_SPAWN;
                for (int i = 0; i < room.SizeOf(); i++)
                {
                    string structure = room.GetBlock(i);
                    if (structure != null)
                    {
                        Vector3 position = room.GetPosition(i);
                        path = "";
                        if (structure.Equals(SPAWN))
                        {
                            if (createDespawn)
                            {
                                createDespawn = false;
                                path = DESPAWN + DIVIDER + styles[roomStyle];
                            }
                            else if (spawnPoint.x == 0 && spawnPoint.y == 0)
                            {
                                path = structure + DIVIDER + styles[roomStyle];
                                spawnPoint = position;
                            }
                            else
                            {
                                path = PLAIN + DIVIDER + styles[roomStyle] + DIVIDER + Random.Range(1, plainsSize);
                            }
                        }
                        else
                        {
                            path = structure + DIVIDER + styles[roomStyle];
                            if (structure.Equals(GROUND))
                            {
                                if (haveChest && Random.Range(0, 2) == 1)
                                {
                                    haveChest = false;
                                    chested = true;
                                }
                                (bool, int) groundChoice = NextGroundEnumerate();
                                if (delaySpawn > 0)
                                    delaySpawn--;
                                if (groundChoice.Item1)
                                {
                                    path = PLAIN + DIVIDER + styles[roomStyle];
                                    if (numberOfSpawns > 0 && delaySpawn == 0 && Random.Range(0, SPAWN_CHANCE) == 1)
                                    {
                                        isSpawnPoint = true;
                                        numberOfSpawns--;
                                        delaySpawn = DELAY_SPAWN;
                                    }
                                }
                                path += DIVIDER + groundChoice.Item2;
                            }
                            else if (structure.Contains(WALL))
                            {
                                int direction = 0;
                                string wallDirection = structure.Substring(4, 2).Replace("_", "");
                                foreach (string wall in wallsName)
                                {
                                    if (wallDirection == wall)
                                    {
                                        break;
                                    }
                                    direction++;
                                }
                                path += DIVIDER + NextWallEnumerate(direction);
                            }
                        }
                        if (stylesIni.Contains(path))
                        {
                            if (chested)
                            {
                                chestPoints.Add(chunkPaths.Count);
                                chested = false;
                            }
                            else if (isSpawnPoint)
                            {
                                spawnPoints.Add(chunkPaths.Count);
                                isSpawnPoint = false;
                            }
                            chunkPaths.Add(FOLDER + styles[roomStyle] + "/" + path);
                            chunkPositions.Add(position);
                        }
                    }
                }
                spawnsPerRoom.Add(roomsCounter, spawnPoints);
                spawnPoints = new List<int>();
                roomsCounter++;
            }
            levelRenderer.AddChunks(chunkPaths, chunkPositions);
            levelRenderer.SetSpawnsPerRoom(spawnsPerRoom);
            levelRenderer.SetChestSpawns(chestPoints);
            levelRenderer.RenderElements();
            levelRenderer.RenderSpawns(new List<int>() { 0 });
            levelRenderer.RenderChests();
            levelRenderer.RenderColliders(map.GetRoomsCollider());
            player.transform.position = spawnPoint;
            player.GetComponent<PlayerObject>().ChangeScene();
            status = true;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        return status;
    }

    #endregion

    #region Numeradores de tiles
    /**
     * Ler iniciadores para os tilemaps definidos dentro da pasta do bioma
     * Executtinga apenas uma vez no inicio do object lifecycle
     */
    void ReadTilesRegistred(string path)
    {
        stylesIni.Clear();
        TextAsset iniFile = Resources.Load<TextAsset>(FOLDER + path);
        string[] lines = iniFile.text.Split('\n');
        plainsSize = 1;
        groundsSize = 1;
        wallsSize = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimEnd('\r', '\n');
            if (!disableVariation)
            {
                if (lines[i].Contains(GROUND))
                {
                    groundsSize++;
                    if (lines[i].Contains(PLAIN))
                    {
                        plainsSize++;
                    }
                }
                else if (lines[i].Contains(WALL))
                {
                    switch (lines[i].Substring(5, 2).Replace("_", ""))
                    {
                        case "SE":
                            wallsSize[0]++;
                            break;
                        case "NE":
                            wallsSize[1]++;
                            break;
                        case "SW":
                            wallsSize[2]++;
                            break;
                        case "NW":
                            wallsSize[3]++;
                            break;
                        case "S":
                            wallsSize[4]++;
                            break;
                        case "E":
                            wallsSize[5]++;
                            break;
                        case "W":
                            wallsSize[6]++;
                            break;
                        case "N":
                            wallsSize[7]++;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        groundsSize -= plainsSize;
        stylesIni.AddRange(lines);
    }

    /**
     * Proximo chão na escolha da sala
     * @return int numero da sala dentro da folder
     */
    (bool, int) NextGroundEnumerate()
    {
        if (groundCounter == randFactor)
        {
            groundCounter++;
            return (false, Random.Range(1, groundsSize + 1));
        }
        if (groundCounter == plainsSize)
        {
            groundCounter = randFactor;
        }
        else if (groundCounter < plainsSize)
        {
            groundCounter++;
        }
        else
        {
            groundCounter = 1;
        }
        return (true, Random.Range(1, plainsSize));
    }

    /**
     * Proxima parede na escolha da sala
     * @return int numero da sala dentro da folder
     */
    int NextWallEnumerate(int direction)
    {
        return randFactor < wallsSize[direction] ?
         Random.Range(randFactor, wallsSize[direction]) : Random.Range(1, wallsSize[direction]);
    }

    #endregion

    #region Geracao de matrix do mapa
    /**
     * Função de mapeamento para gerar posicoes das salas em uma grade
     * de mapa para 4x4 matricial (16 salas) utilizando corte de arestas
     */
    Task<int[]> GenerateMapIndexes()
    {
        int[] matrix = new int[numOfRooms];
        int index = 1, selected = 0;
        List<int> cuttingPool = new List<int>();
        List<int> sideChoices = new List<int> { 0, 1, 2, 3 };
        bool ignore = false;
        pathRooms.Clear();
        origin = (origin != -1 ? origin : UnityEngine.Random.Range(0, 15));
        matrix[0] = origin;
        cuttingPool.Add(origin);
        int cuttingElement = cuttingPool[0], op = 0, realSize = 1;
        do
        {
            op = sideChoices[Random.Range(0, sideChoices.Count)];
            ignore = false;
            if (op == 0)
            {
                if (!MATRIX_LEFT_ELEMENTS.Contains(cuttingElement))
                    selected = cuttingElement - 1;
                else
                    ignore = true;
            }
            else if (op == 1)
            {
                if (!MATRIX_RIGHT_ELEMENTS.Contains(cuttingElement))
                    selected = cuttingElement + 1;
                else
                    ignore = true;
            }
            else if (op == 2)
                selected = cuttingElement + 4;
            else
                selected = cuttingElement - 4;
            if (!ignore && index != numOfRooms && (selected >= 0 && selected <= 15) && !System.Array.Exists(matrix, element => element == selected))
            {
                matrix[index] = selected;
                cuttingPool.Add(selected);
                pathRooms.Add((cuttingElement, selected));
                cuttingElement = cuttingPool[Random.Range(0, cuttingPool.Count)];
                realSize++;
                if (cuttingElement == matrix[index] && sideChoices.Count == 1)
                    cuttingElement = selected;
                else
                    sideChoices = new List<int> { 0, 1, 2, 3 };
                index++;
            }
            else
            {
                if (sideChoices.Count > 0)
                {
                    sideChoices.Remove(op);
                    if (sideChoices.Count == 0)
                    {
                        cuttingPool.Remove(cuttingElement);
                        if (cuttingPool.Count > 0)
                        {
                            cuttingElement = cuttingPool[Random.Range(0, cuttingPool.Count)];
                            sideChoices = new List<int> { 0, 1, 2, 3 };
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

    #endregion

    #region Geracao de portas
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
                intersection = (diff == 1) ? 1 :
                               (diff == -1) ? 2 :
                               (diff == 4) ? 3 :
                               (diff == -4) ? 4 : 0;
            }
            else
            {
                diff = path.Item1 - path.Item2;
                intersection = (diff == 1) ? 2 :
                               (diff == -1) ? 1 :
                               (diff == 4) ? 4 :
                               (diff == -4) ? 3 : 0;
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
            bool allSequencial = true;
            int option = 0;
            //Quando por portas first e last adicionar paredes adjacentes (SE,SW,NE,NW)
            if (targets.Count >= DOORS_LIMIT)
            {
                int divider = targets.Count / 3;
                for (int i = divider; i < targets.Count - divider; i++)
                {
                    (int, int) block = targets[i];
                    if (i == divider)
                    {
                        option = 1;
                    }
                    else if (i == (targets.Count - divider) - 1)
                    {
                        option = 2;
                    }
                    else
                    {
                        option = 3;
                    }
                    UpdateDoors(room1, room2, intersection, block, option);
                }
            }
            else
            {
                if (targets.Count == 1)
                {
                    (int, int) block = targets[0];
                    option = 0;
                    UpdateDoors(room1, room2, intersection, block, option);
                }
                else if (targets.Count == 2)
                {
                    option = -1;
                    for (int i = 0; i < targets.Count; i++)
                    {
                        (int, int) block = targets[i];
                        UpdateDoors(room1, room2, intersection, block, option, i);
                    }
                }
                else
                {
                    int lastValue = -1;
                    foreach ((int, int) block in targets)
                    {
                        if (lastValue != -1)
                        {
                            if (!((block.Item1 == lastValue + 1) || (block.Item1 == lastValue + 20)))
                            {
                                allSequencial = false;
                                break;
                            }
                        }

                        lastValue = block.Item1;
                    }
                    if (!allSequencial)
                    {
                        List<int> newTargets = new List<int>();
                        lastValue = -1;
                        for (int i = 0; i < targets.Count; i++)
                        {
                            newTargets.Add(i);
                            (int, int) block = targets[i];
                            if (lastValue != -1)
                            {
                                int difference = block.Item1 - lastValue;
                                if (difference == 1 || difference == 20)
                                {
                                    lastValue = block.Item1;
                                }
                                else
                                {
                                    ProcessTargets(room1, room2, intersection, targets, newTargets, i);
                                    newTargets.Clear();
                                    newTargets.Add(i);
                                    lastValue = targets[i].Item1;
                                }
                            }
                            else
                            {
                                lastValue = block.Item1;
                            }
                        }
                        ProcessTargets(room1, room2, intersection, targets, newTargets, targets.Count);
                    }
                }
                if (allSequencial)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        (int, int) block = targets[i];
                        if (i == 0)
                        {
                            option = 1;
                        }
                        else if (i == targets.Count - 1)
                        {
                            option = 2;
                        }
                        else
                        {
                            option = 3;
                        }
                        UpdateDoors(room1, room2, intersection, block, option, option == -1 ? i : -1);
                    }
                    allSequencial = false;
                }
            }
        }
    }


    void ProcessTargets(Room room1, Room room2, int intersection, List<(int, int)> targets, List<int> newTargets, int atIndex)
    {
        if (targets.Count == atIndex ? newTargets.Count == 1 : newTargets.Count == 2)
        {
            int index = newTargets[0];
            (int, int) block = targets[index];
            int option = 0;
            UpdateDoors(room1, room2, intersection, block, option);
        }
        else if (targets.Count == atIndex ? newTargets.Count == 2 : newTargets.Count == 3)
        {
            int option = -1, sz = targets.Count == atIndex ? newTargets.Count : newTargets.Count - 1;
            for (int j = 0; j < sz; j++)
            {
                int index = newTargets[j];
                (int, int) block = targets[index];
                UpdateDoors(room1, room2, intersection, block, option, j);
            }
        }
        else
        {
            for (int j = 0; j < newTargets.Count - 1; j++)
            {
                int index = newTargets[j];
                (int, int) block = targets[index];
                int option;
                if (j == 0)
                {
                    option = 1;
                }
                else if (j == newTargets.Count - 2)
                {
                    option = 2;
                }
                else
                {
                    option = 3;
                }
                UpdateDoors(room1, room2, intersection, block, option, option == -1 ? j : -1);
            }
        }
    }

    private void UpdateDoors(Room room1, Room room2, int intersection, (int, int) block, int option, int secondIndex = -1)
    {
        if (intersection == 1)
        {
            switch (option)
            {
                case -1:
                    if (secondIndex == 0)
                    {
                        room1.UpdateBlock("WALL_SW", block.Item1);
                        room2.UpdateBlock(DOORS[2], block.Item2);
                    }
                    else
                    {
                        room1.UpdateBlock("WALL_NE", block.Item1);
                        room2.UpdateBlock(DOORS[1], block.Item2);
                    }
                    break;
                case 0:
                    room1.UpdateBlock(DOORS[0], block.Item1);
                    room2.UpdateBlock(DOORS[0], block.Item2);
                    break;
                case 1:
                    room1.UpdateBlock("WALL_SW", block.Item1);
                    room2.UpdateBlock(DOORS[2], block.Item2);
                    break;
                case 2:
                    room1.UpdateBlock("WALL_NE", block.Item1);
                    room2.UpdateBlock(DOORS[1], block.Item2);
                    break;
                case 3:
                    room1.UpdateBlock(GROUND, block.Item1);
                    room2.UpdateBlock(DOORS[3], block.Item2);
                    break;
                default:
                    break;
            }
        }
        else if (intersection == 2)
        {
            switch (option)
            {
                case -1:
                    if (secondIndex == 0)
                    {
                        room1.UpdateBlock(DOORS[2], block.Item1);
                        room2.UpdateBlock("WALL_SW", block.Item2);
                    }
                    else
                    {
                        room1.UpdateBlock(DOORS[1], block.Item1);
                        room2.UpdateBlock("WALL_NE", block.Item2);
                    }
                    break;
                case 0:
                    room1.UpdateBlock(DOORS[0], block.Item1);
                    room2.UpdateBlock(DOORS[0], block.Item2);
                    break;
                case 1:
                    room1.UpdateBlock(DOORS[2], block.Item1);
                    room2.UpdateBlock("WALL_SW", block.Item2);
                    break;
                case 2:
                    room1.UpdateBlock(DOORS[1], block.Item1);
                    room2.UpdateBlock("WALL_NE", block.Item2);
                    break;
                case 3:
                    room1.UpdateBlock(DOORS[3], block.Item1);
                    room2.UpdateBlock(GROUND, block.Item2);
                    break;
                default:
                    break;
            }
        }
        else if (intersection == 3)
        {
            switch (option)
            {
                case -1:
                    if (secondIndex == 0)
                    {
                        room1.UpdateBlock("WALL_SE", block.Item1);
                        room2.UpdateBlock(DOORS[6], block.Item2);
                    }
                    else
                    {
                        room1.UpdateBlock("WALL_NW", block.Item1);
                        room2.UpdateBlock(DOORS[5], block.Item2);
                    }
                    break;
                case 0:
                    room1.UpdateBlock(DOORS[4], block.Item1);
                    room2.UpdateBlock(DOORS[4], block.Item2);
                    break;
                case 1:
                    room1.UpdateBlock("WALL_SE", block.Item1);
                    room2.UpdateBlock(DOORS[6], block.Item2);
                    break;
                case 2:
                    room1.UpdateBlock("WALL_NW", block.Item1);
                    room2.UpdateBlock(DOORS[5], block.Item2);
                    break;
                case 3:
                    room1.UpdateBlock(GROUND, block.Item1);
                    room2.UpdateBlock(DOORS[7], block.Item2);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (option)
            {
                case -1:
                    if (secondIndex == 0)
                    {
                        room1.UpdateBlock(DOORS[6], block.Item1);
                        room2.UpdateBlock("WALL_SE", block.Item2);
                    }
                    else
                    {
                        room1.UpdateBlock(DOORS[5], block.Item1);
                        room2.UpdateBlock("WALL_NW", block.Item2);
                    }
                    break;
                case 0:
                    room1.UpdateBlock(DOORS[4], block.Item1);
                    room2.UpdateBlock(DOORS[4], block.Item2);
                    break;
                case 1:
                    room1.UpdateBlock(DOORS[6], block.Item1);
                    room2.UpdateBlock("WALL_SE", block.Item2);
                    break;
                case 2:
                    room1.UpdateBlock(DOORS[5], block.Item1);
                    room2.UpdateBlock("WALL_NW", block.Item2);
                    break;
                case 3:
                    room1.UpdateBlock(DOORS[7], block.Item1);
                    room2.UpdateBlock(GROUND, block.Item2);
                    break;
                default:
                    break;
            }
        }
    }

    /**
     * Definir portas usando regras e listas de MAX definidos na classe ROOM
     * Encontrar posições semelhantes para remover walls e adicionar doors
     * Pesquisando por interseções entre as salas que contem mesmo indice na ordem de inserção
     */
    List<(int, int)> GetTargetDoors(List<(int, int)> maxElementsRoom1, List<(int, int)> maxElementsRoom2)
    {
        List<(int, int)> targets = new List<(int, int)>();
        foreach ((int, int) target in maxElementsRoom1)
        {
            foreach ((int, int) target2 in maxElementsRoom2)
            {
                if (target.Item2 == target2.Item2)
                {
                    targets.Add((target.Item1, target2.Item1));
                }

            }
        }
        return targets;
    }

    #endregion
}