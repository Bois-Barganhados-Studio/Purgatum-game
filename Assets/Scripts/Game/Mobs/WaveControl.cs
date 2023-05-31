using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl
{
    #region Variaveis/Gettters/Setters
    private int currentWave = 1, max_waves, min_waves,
    min_enemies, max_enemies;
    private static List<GameObject> loadedEnemiesObjects = new List<GameObject>();
    private static List<int> loadedEnemiesIndexes = new List<int>();
    private List<(int, string)> avaliableEnemies = new List<(int, string)> { (0, "Prefab/Game/Entities/Enemy"), (1, "Assets/Prefabs/Enemies/Enemy2") };

    public WaveControl(int max_waves, int min_waves, int min_enemies, int max_enemies)
    {
        this.max_waves = max_waves;
        this.min_waves = min_waves;
        this.min_enemies = min_enemies;
        this.max_enemies = max_enemies;
        if (loadedEnemiesObjects.Count == 0)
        {
            Debug.Log("LOADING ENEMY FROM DISK");
            foreach ((int, string) enemy in avaliableEnemies)
            {
                loadedEnemiesObjects.Add(Resources.Load(enemy.Item2) as GameObject);
                loadedEnemiesIndexes.Add(enemy.Item1);
            }
        }
    }

    #endregion


    #region Controladores

    //<sumary>
    // Busca dos inimigos relacionados pelo tipo para poder spawnar
    //</sumary>
    public List<GameObject> GetEnemies(int enemyType)
    {
        List<GameObject> enemies = new List<GameObject>();
        int enemiesCount = Random.Range(min_enemies, GetmaxEnemies()), targetEnemy = 0;
        if (loadedEnemiesIndexes.Contains(enemyType))
        {
            targetEnemy = loadedEnemiesIndexes.IndexOf(enemyType);
        }
        for (int i = 0; i < enemiesCount; i++)
        {
            enemies.Add(GetEnemy(targetEnemy) ?? GetEnemy(0));
        }
        return enemies;
    }

    //<sumary>
    // Avanco para proxima wave de spawns de inimigos
    //</sumary>
    public void nextWave()
    {
        currentWave++;
    }

    //<sumary>
    // maximo de inimigos por wave que podem ser spawnados
    //</sumary>
    public int GetmaxEnemies()
    {
        int prob = Random.Range(1, currentWave);
        if (prob == max_waves)
        {
            return max_enemies;
        }
        else
        {
            return (max_enemies - prob) < min_enemies ? min_enemies : (max_enemies - prob);
        }
    }


    private GameObject GetEnemy(int enemyType)
    {
        if (loadedEnemiesObjects.Count - 1 > enemyType)
        {
            return loadedEnemiesObjects[enemyType];
        }
        return null;
    }

    #endregion


}
