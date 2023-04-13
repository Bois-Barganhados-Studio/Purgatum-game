using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteObject : MonoBehaviour
{
    void EndDodge()
    {
        Debug.Log("callback");
        FindObjectOfType<PlayerObject>().EndDodge();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
