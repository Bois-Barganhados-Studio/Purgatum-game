using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnManager
{
    public event Action OnAllSpawnsFinished;
    private List<Spawner> activeSpawns = new List<Spawner>();
    private const int ITEM_DROP_CHANCE = 22;
    private static GameObject chest = null;

    public SpawnManager(List<Spawner> spawns)
    {
        foreach (Spawner spawn in spawns)
        {
            if (spawn.IsCommander())
            {
                spawn.OnSpawnsFinished += HandleSpawnFinished;
                activeSpawns.Add(spawn);
            }
        }
        if (chest == null)
        {
            chest = Resources.Load<GameObject>("Prefab/Game/Enviroment/Chest");
        }
        Debug.Log("Active spawns: " + activeSpawns.Count);
    }

    private void HandleSpawnFinished(Spawner spawn)
    {
        activeSpawns.Remove(spawn);
        if (UnityEngine.Random.Range(0, 100) <= ITEM_DROP_CHANCE)
        {
            SpawnChest(spawn.gameObject);
        }
        Debug.Log("COMMANDERS RESTANTES: " + activeSpawns.Count);
        if (activeSpawns.Count == 0)
        {
            OnAllSpawnsFinished?.Invoke();
        }
    }

    private void SpawnChest(GameObject spGameObject)
    {
        GameObject.Instantiate(chest, new Vector3(spGameObject.transform.position.x + 0.4f,
        spGameObject.transform.position.y, spGameObject.transform.position.z),
        Quaternion.identity, spGameObject.transform);
    }
}