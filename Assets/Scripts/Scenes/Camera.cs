using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerObject>().transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0, 0, -5);
        }
        else
        {
            player = GameObject.FindObjectOfType<PlayerObject>().transform;
        }
    }
}
