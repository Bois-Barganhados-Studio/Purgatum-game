using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLogic
{
    #region Variaveis/Gettters/Setters
    private int enemyType;
    private WaveControl waveControl;
    public SpawnLogic(int enemyType = 0)
    {
        this.enemyType = enemyType;
        waveControl = new WaveControl();
    }

    #endregion


    #region Controladores

    //<sumary>
    // Build do Spawn de inimigos 
    //</sumary>
    public List<GameObject> GetSpawnableEnemies()
    {
        List<GameObject> enemies = waveControl.GetEnemies(enemyType);
        waveControl.nextWave();
        return enemies;
    }


    #endregion
}
