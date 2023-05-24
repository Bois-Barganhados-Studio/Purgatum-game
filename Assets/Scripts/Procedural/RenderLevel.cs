using UnityEngine;
using System.Collections.Generic;

/**
 * @author Leon Jr
 */
public class RenderLevel
{
    private static List<GameObject> level = new List<GameObject>();
    private static List<(GameObject, Vector3)> chunks = new List<(GameObject, Vector3)>();
    private static Dictionary<string, GameObject> loadedTiles = new Dictionary<string, GameObject>();
    public static GameObject ROOM_COLLIDER_PREFAB = null;
    private const string ROOM_COLLIDER = "RoomCollider";
    public static GameObject renderLevels = null;
    public float x_min = 9999999, y_min = 9999999, x_max = -9999999, y_max = -9999999;

    public RenderLevel()
    {
        renderLevels = GameObject.Find("renderLevels");
        ROOM_COLLIDER_PREFAB = Resources.Load<GameObject>("Maps/" + ROOM_COLLIDER);
    }

    /*
    * Busca o gameobject da Sala em questão
    */
    public static GameObject GetRoomObject(int index)
    {
        return level[index];
    }

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

    public void RenderColliders(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            GameObject.Instantiate(ROOM_COLLIDER_PREFAB, position, Quaternion.identity, renderLevels.transform);
        }
    }

    /**
     * Renderiza��o do tilemap no object de renderLevels
     */
    GameObject GenGameTileMap(GameObject obj, Vector3 pos)
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
}
