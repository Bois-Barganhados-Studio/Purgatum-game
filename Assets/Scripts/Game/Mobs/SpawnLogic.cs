using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLogic
{
    #region Variaveis/Gettters/Setters
    private int enemyType;
    private bool isCommander;
    private WaveControl waveControl;
    public static readonly int MAX_WAVES_COMMANDER = 5, MAX_WAVES_DEFAULT = 3, MIN_WAVES_COMMANDER = 3, MIN_WAVES_DEFAULT = 1,
    MIN_ENEMIES_DEFAULT = 2, MAX_ENEMIES_DEFAULT = 6, MIN_ENEMIES_COMMANDER = 5, MAX_ENEMIES_COMMANDER = 8;
    public SpawnLogic(int enemyType = 0, bool IsCommander = false)
    {
        this.isCommander = IsCommander;
        this.enemyType = enemyType;
        if (isCommander)
        {
            this.waveControl = new WaveControl(max_waves: MAX_WAVES_COMMANDER, min_waves: MIN_WAVES_COMMANDER,
             min_enemies: MIN_ENEMIES_COMMANDER, max_enemies: MAX_ENEMIES_COMMANDER);
        }
        else
        {
            this.waveControl = new WaveControl(max_waves: MAX_WAVES_DEFAULT, min_waves: MIN_WAVES_DEFAULT,
             min_enemies: MIN_ENEMIES_DEFAULT, max_enemies: MAX_ENEMIES_DEFAULT);
        }
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
