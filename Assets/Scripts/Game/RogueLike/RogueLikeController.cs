using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueLikeController : MonoBehaviour
{

    RogueLogic rogueLogic = new RogueLogic();

    void Start()
    {
        //Invoke("NewLevel", 10f);
    }

    public void NewLevel()
    {
        rogueLogic.DoAction();
    }
    
}
