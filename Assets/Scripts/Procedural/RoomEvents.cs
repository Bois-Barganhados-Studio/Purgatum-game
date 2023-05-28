using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvents : MonoBehaviour
{
    bool visited = false;
    private void OnTriggerEnter2D(Collider2D colliderElement)
    {
        Debug.Log("Something entered the room! " + colliderElement.gameObject.name + " " + colliderElement.gameObject.layer + "!");
        if (!visited && Player.LAYER == colliderElement.gameObject.layer)
        {
            Debug.Log("Player entered the room!");
            visited = true;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(RoomActions());
        }
    }

    private IEnumerator RoomActions()
    {
        TurnOnDoors();
        SpawnMobs();
        TurnOnLights();
        yield return null;
    }

    private void TurnOnDoors()
    {
        Debug.Log("Fechou Portas!");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("DOOR");
        foreach (GameObject door in doors)
        {
            door.GetComponent<Doors>().SetState(true);
        }
    }

    private void SpawnMobs()
    {
        Debug.Log("Spawnou Mobs!");
    }

    private void TurnOnLights()
    {
        Debug.Log("Acendeu Luzes!");
    }
}