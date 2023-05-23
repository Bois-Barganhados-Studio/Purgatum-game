using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueLikeController : MonoBehaviour
{
    
    //definir dados

    //definir logica


    void Start()
    {
       // Invoke("NewLevel", 15f);
    }

    void NewLevel()
    {
        GameObject gobj = GameObject.Find("renderLevels");
        gobj.GetComponent<ProceduralMapBuilder>().NewLevel();
    }
}
