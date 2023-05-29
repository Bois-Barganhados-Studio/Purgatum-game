using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variaveis
    private Vector2 spawnPosition;
    private SpawnLogic spawnLogic;
    private int enemyType = 0;
    private float spawnTime = 6f;
    private bool isSpawning = false;
    public event System.Action<Spawner> OnSpawnsFinished;

    #endregion

    #region Controladores

    void Awake()
    {
        this.spawnPosition = transform.position;
    }

    public void SetEnemyType(int enemyType)
    {
        this.enemyType = enemyType;
    }

    public void SetSpawnTime(float spawnTime)
    {
        this.spawnTime = spawnTime;
    }

    /**
    * Inicia o spawn de inimigos apenas uma vez por spawn point
    */
    public void StartSpawner()
    {
        if (!isSpawning)
        {
            spawnLogic = new SpawnLogic(enemyType: enemyType);
            isSpawning = true;
            StartCoroutine(Spawn());
        }
    }

    /*
    * Spawn de inimigos como corrotina assincrona
    */
    private IEnumerator Spawn()
    {
        int waves = Random.Range(WaveControl.MIN_WAVES, WaveControl.MAX_WAVES), counter = 0;
        while (counter != waves)
        {
            yield return new WaitForSeconds(spawnTime);
            int sz = RenderEnemies(spawnLogic.GetSpawnableEnemies());
            yield return StartCoroutine(WaitForEnemiesDestroyed(transform.childCount - sz));
            counter++;
            Debug.Log("SPAWNING MORE ENEMIES. WAVES: " + counter + " / " + waves);
        }
        OnSpawnsFinished?.Invoke(this);
        yield return null;
        Debug.Log("SPAWN ENDED");
        Destroy(this);
    }

    private IEnumerator WaitForEnemiesDestroyed(int removeCount = 1)
    {
        while (transform.childCount - removeCount > 0)
        {
            yield return null;
        }
        Debug.Log("ALL ENEMIES DESTROYED");
    }

    //<sumary>
    // Renderiza o inimigo individual na tela
    //</sumary>
    private int RenderEnemies(List<GameObject> enemies)
    {
        // float multiplier = 0.2f;
        foreach (GameObject enemy in enemies)
        {
            GameObject.Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
            //multiplier += 0.1f;
        }
        return enemies.Count;
    }
    #endregion

}
