using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour
{
    // Start is called before the first frame update
    void RuntimeInitializeOnLoadMethod()
    {
        PlayerObject player = GameObject.FindObjectOfType<PlayerObject>();
        if(player != null)
        {
            Debug.Log("awake HUDasoda");
            player.ChangeScene();
            player.transform.position = new Vector3(0.9f,1.153f,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
