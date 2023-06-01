using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvents : MonoBehaviour
{
    private bool visited = false;
    public int ROOM = 0;
    private List<Spawner> avaliableSpawns;
    private SpawnManager spawnManager;
    private const string DOOR = "DOOR", LIGHT = "LIGHT";
    public void SetAvaliableSpawns(List<Spawner> avaliableSpawns)
    {
        this.avaliableSpawns = avaliableSpawns;
    }
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
        GameObject[] doors = GameObject.FindGameObjectsWithTag(DOOR);
        foreach (GameObject door in doors)
        {
            door.GetComponent<Doors>().SetState(false);
        }
    }

    private void TurnOffDoors()
    {
        Debug.Log("Abriu Portas!");
        GameObject[] doors = GameObject.FindGameObjectsWithTag(DOOR);
        foreach (GameObject door in doors)
        {
            door.GetComponent<Doors>().SetState(true);
        }
    }

    private void SpawnMobs()
    {
        if (avaliableSpawns != null)
        {
            foreach (Spawner spawn in avaliableSpawns)
            {
                spawn.StartSpawner();
            }
            spawnManager = new SpawnManager(avaliableSpawns);
            spawnManager.OnAllSpawnsFinished += TurnOffDoors;
        }
    }

    private void TurnOnLights()
    {
        Debug.Log("Acendeu Luzes!");
        GameObject[] lights = GameObject.FindGameObjectsWithTag(LIGHT);
        foreach (GameObject light in lights)
        {
            light.GetComponent<Torch>().TurnTorch(true);
        }
    }

    public bool IsVisited()
    {
        return visited;
    }
}