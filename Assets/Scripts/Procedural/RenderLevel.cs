using UnityEngine;
using System.Collections.Generic;

/**
 * @author Leon Jr
 */
public class RenderLevel
{
    #region Variaveis
    private static List<GameObject> level = new List<GameObject>();
    private static List<(GameObject, Vector3)> chunks = new List<(GameObject, Vector3)>();
    private static Dictionary<string, GameObject> loadedTiles = new Dictionary<string, GameObject>();
    private Dictionary<int, List<int>> spawnsPerRoom;
    private Dictionary<int, List<Spawner>> spawnsCreatedPerRoom = new Dictionary<int, List<Spawner>>();
    private static List<Spawner> spawnsCreated = new List<Spawner>();
    public static GameObject ROOM_COLLIDER_PREFAB = null;
    private const string ROOM_COLLIDER = "RoomCollider";
    public static GameObject renderLevels = null;
    public float x_min = 9999999, y_min = 9999999, x_max = -9999999, y_max = -9999999;
    #endregion

    #region Contrutores e getters
    public RenderLevel()
    {
        renderLevels = GameObject.Find("renderLevels");
        ROOM_COLLIDER_PREFAB = Resources.Load<GameObject>("Maps/" + ROOM_COLLIDER);
    }

    public void SetSpawnsPerRoom(Dictionary<int, List<int>> spawnsPerRoom)
    {
        this.spawnsPerRoom = spawnsPerRoom;
    }

    /**
    * Adicionando chunks para load de objetos da pasta resources 
    */
    public void AddChunks(List<string> chunkPaths, List<Vector3> chunkPositions)
    {
        for (int i = 0; i < chunkPaths.Count; i++)
        {
            if (!loadedTiles.ContainsKey(chunkPaths[i]))
            {
                loadedTiles.Add(chunkPaths[i], Resources.Load<GameObject>(chunkPaths[i]));
            }
            chunks.Add((loadedTiles[chunkPaths[i]], chunkPositions[i]));
        }
    }
    #endregion

    #region Cleaners
    /**
     * Limpa os chunks salvos na memoria
     */
    public void UnloadMemory()
    {
        chunks.Clear();
        loadedTiles.Clear();
    }

    /**
     * Limpa o gameobject de renderLevels
     */
    public void ClearGameObject()
    {
        if (renderLevels != null)
        {
            for (int i = renderLevels.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(renderLevels.transform.GetChild(i).gameObject);
            }
        }
        level.Clear();
        x_min = 9999999;
        y_min = 9999999;
        x_max = -9999999;
        y_max = -9999999;
    }
    #endregion

    #region Renderizadores
    /**
     * Renderizando elementos na tela do jogo
     */
    public void RenderElements()
    {
        foreach ((GameObject, Vector3) tuple in chunks)
        {
            level.Add(GenGameTileMap(tuple.Item1, tuple.Item2));
        }
    }

    /**
     * Renderizacao de spawns no mapa
     */
    public void RenderSpawns(List<int> enemiesType)
    {
        spawnsCreated.Clear();
        foreach (KeyValuePair<int, List<int>> entry in spawnsPerRoom)
        {
            Debug.Log(entry.Key + " " + entry.Value.Count);
            foreach (int spawnPoint in entry.Value)
            {
                Spawner spawn = level[spawnPoint].AddComponent<Spawner>();
                spawn.SetEnemyType(enemiesType[Random.Range(0, enemiesType.Count)]);
                spawn.SetSpawnTime(0.1f);
                spawnsCreated.Add(spawn);
            }
            spawnsCreatedPerRoom.Add(entry.Key, spawnsCreated);
            spawnsCreated = new List<Spawner>();
        }
        Debug.Log("sp ct: " + spawnsCreated.Count);
    }
    public void RenderColliders(List<Vector3> positions)
    {
        int roomIndex = 0;
        foreach (Vector3 position in positions)
        {
            GameObject gobj = GameObject.Instantiate(ROOM_COLLIDER_PREFAB, position, Quaternion.identity, renderLevels.transform);
            RoomEvents room = gobj.GetComponent<RoomEvents>();
            room.ROOM = roomIndex++;
            room.SetAvaliableSpawns(spawnsCreatedPerRoom[room.ROOM]);
        }
    }

    /**
     * Renderizacao do tilemap no object de renderLevels
     */
    private GameObject GenGameTileMap(GameObject obj, Vector3 pos)
    {
        if (pos.x < x_min)
        {
            x_min = pos.x;
        }
        if (pos.x > x_max)
        {
            x_max = pos.x;
        }
        if (pos.y < y_min)
        {
            y_min = pos.y;
        }
        if (pos.y > y_max)
        {
            y_max = pos.y;
        }
        GameObject tilemap = GameObject.Instantiate(obj, pos, Quaternion.identity, renderLevels.transform);
        tilemap.transform.parent = renderLevels.transform;
        return tilemap;
    }
    #endregion

}
