using UnityEngine;
using System.Collections.Generic;

/**
 * @author Leon Jr
 */
public class RenderLevel
{
    private List<GameObject> level = new List<GameObject>();
    private List<(GameObject, Vector3)> chunks = new List<(GameObject, Vector3)>();
    private GameObject renderLevels = null;


    public RenderLevel()
    {
        renderLevels = GameObject.Find("renderLevels");
    }

    /**
     * Adicionando chunks para load de objetos da pasta resources 
     */
    public void AddChunks(List<string> chunkPaths, List<Vector3> chunkPositions)
    {
        for (int i = 0; i < chunkPaths.Count; i++)
        {
            chunks.Add((Resources.Load<GameObject>(chunkPaths[i]), chunkPositions[i]));
        }
    }

    /**
     * Renderizando elementos na tela do jogo
     */
    public void RenderElements()
    {
        Debug.Log("Rendering chunks in the level");
        // Remove all existing children of renderLevels
        foreach (Transform child in renderLevels.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach ((GameObject, Vector3) tuple in chunks)
        {
            level.Add(GenGameTileMap(tuple.Item1, tuple.Item2));
        }
    }

    /**
     * Renderização do tilemap no object de renderLevels
     */
    private GameObject GenGameTileMap(GameObject obj, Vector3 pos)
    {
        GameObject tilemap = GameObject.Instantiate(obj, pos, Quaternion.identity, renderLevels.transform);
        tilemap.transform.parent = renderLevels.transform;
        return tilemap;
    }
}
