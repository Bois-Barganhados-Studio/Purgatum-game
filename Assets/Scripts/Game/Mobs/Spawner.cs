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

    private const int MAX_RANGE = 15, MIN_RANGE = -15;
    private Vector2 rangeEnemyPos = Vector2.zero;

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
            List<GameObject> enemies = spawnLogic.GetSpawnableEnemies();
            int sz = enemies.Count;
            foreach (GameObject enemy in enemies)
            {
                RenderEnemy(enemy);
                yield return new WaitForSeconds(spawnTime);
            }
            yield return StartCoroutine(WaitForEnemiesDestroyed());
            counter++;
            Debug.Log("SPAWNING MORE ENEMIES. WAVES: " + counter + " / " + waves);
        }
        OnSpawnsFinished?.Invoke(this);
        yield return null;
        Debug.Log("SPAWN ENDED");
        Destroy(this);
    }

    private IEnumerator WaitForEnemiesDestroyed()
    {
        while (transform.childCount - 1 > 0)
        {
            yield return null;
        }
        Debug.Log("ALL ENEMIES DESTROYED");
    }

    //<sumary>
    // Renderiza o inimigo individual na tela
    //</sumary>
    private void RenderEnemy(GameObject enemy)
    {
        rangeEnemyPos = new Vector2((Random.Range(MIN_RANGE, MAX_RANGE) / 10), (Random.Range(MIN_RANGE, MAX_RANGE) / 10));
        GameObject.Instantiate(enemy, spawnPosition + rangeEnemyPos, Quaternion.identity, transform);
    }
    #endregion

}
