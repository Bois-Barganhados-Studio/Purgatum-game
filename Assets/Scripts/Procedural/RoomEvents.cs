using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvents : MonoBehaviour
{
    bool visited = false;
    private void OnTriggerEnter2D(Collider2D colliderElement)
    {
        if (!visited && Player.LAYER == (Player.LAYER | (1 << colliderElement.gameObject.layer)))
        {
            Debug.Log("Player entered the room!");
            visited = true;
            //desligar colisão com o player
        }
    }
}