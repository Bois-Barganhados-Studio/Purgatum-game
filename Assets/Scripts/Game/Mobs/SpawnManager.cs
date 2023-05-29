using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnManager
{
    public event Action OnAllSpawnsFinished;
    private List<Spawner> activeSpawns = new List<Spawner>();

    public SpawnManager(List<Spawner> spawns)
    {
        foreach (Spawner spawn in spawns)
        {
            spawn.OnSpawnsFinished += HandleSpawnFinished;
            activeSpawns.Add(spawn);
        }
    }

    private void HandleSpawnFinished(Spawner spawn)
    {
        Debug.Log("Spawn finished so remove it from active spawns");
        activeSpawns.Remove(spawn);
        if (activeSpawns.Count == 0)
        {
            OnAllSpawnsFinished?.Invoke();
        }
    }
}




