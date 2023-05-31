using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private float timeToDespawn = 0f;

    void Awake()
    {
        this.timeToDespawn = 2f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //todo: perguntar se o player quer sair
            Debug.Log("GOING TO NEXT LEVEL");
            StartCoroutine(DespawnPlayer());
        }
    }

    private IEnumerator DespawnPlayer()
    {
        yield return new WaitForSeconds(this.timeToDespawn);
        RogueLikeController.OnGoingToNextLevel();
    }
}