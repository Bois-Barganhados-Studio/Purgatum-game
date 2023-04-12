using UnityEngine;
using System.Collections;

public class RenderLevels : MonoBehaviour
{
    public string style = "Maps/GROUND_STONE_1";
    public string styleT = "Maps/GROUND_STONE_2";
    public Vector3 gPosition = new(5f, 0, 0); //float numbers
    public Vector3 gxPosition = new(0f, 5f, 0); //float numbers
    public ArrayList list = new();

    void Start()
    {
        Debug.Log("Running");
        Debug.Log(style);
        GameObject styleObj = (GameObject)Resources.Load(style);
        GameObject styleObjT = (GameObject)Resources.Load(styleT);
        Debug.Log(styleObj);
        Debug.Log(styleObjT);
        list.Add(GenGameTileMap(styleObj, gPosition));
        list.Add(GenGameTileMap(styleObjT, gxPosition));
    }


    GameObject GenGameTileMap(GameObject obj, Vector3 pos)
    {
        GameObject tilemap = Instantiate(obj, pos, Quaternion.identity);
        tilemap.transform.parent = transform;
        return tilemap;
    }
}