using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HubExit : MonoBehaviour
{
    private float timeToDespawn = 0f;
    void Awake()
    {
        this.timeToDespawn = 0.5f;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("FIRST LEVEL STARTING");
            StartCoroutine(DespawnPlayer());
        }
    }
    IEnumerator DespawnPlayer()
    {
        yield return new WaitForSeconds(this.timeToDespawn);
        GameObject.FindObjectOfType<RogueLikeController>().OnFirstRun();
    }
}