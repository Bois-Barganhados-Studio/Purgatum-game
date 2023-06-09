using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private float timeToDespawn = 0f;

    void Awake()
    {
        this.timeToDespawn = 1f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (RoomEvents room in GameObject.FindObjectsOfType<RoomEvents>())
            {
                if (!room.IsVisited())
                {
                    Debug.Log("Sala não completa ainda!");
                    return;
                }
            }
            StartCoroutine(DespawnPlayer());
        }
    }

    private IEnumerator DespawnPlayer()
    {
        yield return new WaitForSeconds(this.timeToDespawn);
        GameObject.FindObjectOfType<RogueLikeController>().OnGoingToNextLevel();
    }
}