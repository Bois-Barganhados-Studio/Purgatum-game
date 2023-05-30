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
    private const int MAX_SPAWN_RANGE = 15, MIN_SPAWN_RANGE = -15;
    private Vector2 rangeEnemyPos = Vector2.zero;
    private bool isCommander = false;

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

    public void SetCommander(bool isCommander)
    {
        this.isCommander = isCommander;
        Debug.Log(isCommander ? "COMMANDER" : "DEFAULT");
    }

    public bool IsCommander()
    {
        return this.isCommander;
    }

    /**
    * Inicia o spawn de inimigos apenas uma vez por spawn point
    */
    public void StartSpawner()
    {
        if (!isSpawning)
        {
            spawnLogic = new SpawnLogic(enemyType: enemyType, IsCommander: isCommander);
            isSpawning = true;
            StartCoroutine(Spawn());
        }
    }

    /*
    * Spawn de inimigos como corrotina assincrona
    */
    private IEnumerator Spawn()
    {
        int waves = (isCommander ? Random.Range(SpawnLogic.MIN_WAVES_COMMANDER, SpawnLogic.MAX_WAVES_COMMANDER) :
        Random.Range(SpawnLogic.MIN_WAVES_DEFAULT, SpawnLogic.MAX_WAVES_DEFAULT)), counter = 0;
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
        if (isCommander)
        {
            OnSpawnsFinished?.Invoke(this);
        }
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
        rangeEnemyPos = new Vector2((Random.Range(MIN_SPAWN_RANGE, MAX_SPAWN_RANGE) / 10), (Random.Range(MIN_SPAWN_RANGE, MAX_SPAWN_RANGE) / 10));
        GameObject.Instantiate(enemy, spawnPosition + rangeEnemyPos, Quaternion.identity, transform);
    }
    #endregion

}
