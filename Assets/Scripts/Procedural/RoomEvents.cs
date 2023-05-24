using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvents : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Player.LAYER == (Player.LAYER | (1 << other.gameObject.layer)))
        {
            Debug.Log("Player entered the room!");
        }
    }
}