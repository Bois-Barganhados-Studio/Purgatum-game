using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl
{
    #region Variaveis/Gettters/Setters
    public static readonly int MAX_WAVES = 5, MIN_WAVES = 1,
    MIN_ENEMIES = 4, MAX_ENEMIES = 12;
    private int currentWave = 1;
    private static List<GameObject> loadedEnemiesObjects = new List<GameObject>();
    private static List<int> loadedEnemiesIndexes = new List<int>();
    private List<(int, string)> avaliableEnemies = new List<(int, string)> { (0, "Prefab/Game/Entities/Enemy"), (1, "Assets/Prefabs/Enemies/Enemy2") };

    public WaveControl()
    {
        // Carrega os inimigos previamente 
        // (pode ser custoso e pode blockar a thread principal)
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
        int enemiesCount = Random.Range(MIN_ENEMIES, GetMaxEnemies()), targetEnemy = 0;
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
    // Maximo de inimigos por wave que podem ser spawnados
    //</sumary>
    public int GetMaxEnemies()
    {
        int prob = Random.Range(1, currentWave);
        if (prob == MAX_WAVES)
        {
            return MAX_ENEMIES;
        }
        else
        {
            return (MAX_ENEMIES - prob) < MIN_ENEMIES ? MIN_ENEMIES : (MAX_ENEMIES - prob);
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
