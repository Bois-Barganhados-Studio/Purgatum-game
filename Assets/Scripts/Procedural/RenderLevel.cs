using UnityEngine;
using System.Collections.Generic;

/**
 * @author Leon Jr
 */
public class RenderLevel
{
    private List<GameObject> level = new List<GameObject>();
    private static List<(GameObject, Vector3)> chunks = new List<(GameObject, Vector3)>();
    private static Dictionary<string, GameObject> loadedTiles = new Dictionary<string, GameObject>();
    private GameObject renderLevels = null;

    public RenderLevel()
    {
        renderLevels = GameObject.Find("renderLevels");
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

    public float x_min = 9999999, y_min = 9999999, x_max = -9999999, y_max = -9999999;

    /**
     * Renderização do tilemap no object de renderLevels
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
