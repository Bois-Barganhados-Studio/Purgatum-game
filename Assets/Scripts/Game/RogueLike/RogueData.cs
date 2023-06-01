using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueData
{
   //armazenar dados para a IA
   public List<WorldData> playerRunsData = null;
   public List<bool> playerRunsResult = null;

   private int deathCount = 0, surviveCount = 0;

   public List<WorldData> playerDeathsData = null;


   public RogueData()
   {
      playerRunsData = new List<WorldData>();
      playerRunsResult = new List<bool>();
      playerDeathsData = new List<WorldData>();
   }
   //Banco inicial para teste
   public void CreateSampleData() {
      WorldData[] sample = new WorldData[15];
      bool[] sampleResult = new bool[15];

      
      
      sample[0] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 0, origin: 4,randFactor: 0, blueprints: new int[] { 0,1,2,3 }), new int[]{0,1,1,3});
      sampleResult[0] = false;
      sample[1] = new WorldData(new LevelData(numOfRooms: 5, roomStyle: 1, origin: 3,randFactor: 0, blueprints: new int[] { 4,3,2,1,5 }), new int[]{4,2,0,1});
      sampleResult[1] = true;
      sample[2] = new WorldData(new LevelData(numOfRooms: 8, roomStyle: 2, origin: 3,randFactor: 0, blueprints: new int[] { 1,1,4,3,2,3,5,1 }), new int[]{4,1,1,3});
      sampleResult[2] = false;
      sample[3] = new WorldData(new LevelData(numOfRooms: 3, roomStyle: 3, origin: 3,randFactor: 0, blueprints: new int[] { 0,1,0 }), new int[]{0,1,0,0});
      sampleResult[3] = true;
      sample[4] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 4, origin: 2,randFactor: 0, blueprints: new int[] { 1,4,2,0 }), new int[]{2,3,1,1});
      sampleResult[4] = false;
      sample[5] = new WorldData(new LevelData(numOfRooms: 5, roomStyle: 5, origin: 1,randFactor: 0, blueprints: new int[] { 0,1,0,1,0 }), new int[]{1,2,1,1});
      sampleResult[5] = false;
      sample[6] = new WorldData(new LevelData(numOfRooms: 9, roomStyle: 2, origin: 4,randFactor: 0, blueprints: new int[] { 0,2,3,4,1,0,5,1,2 }), new int[]{1,2,2,3});
      sampleResult[6] = false;
      sample[7] = new WorldData(new LevelData(numOfRooms: 2, roomStyle: 2, origin: 5,randFactor: 0, blueprints: new int[] { 0,5 }), new int[]{2,3,4,3});
      sampleResult[7] = true;
      sample[8] = new WorldData(new LevelData(numOfRooms: 3, roomStyle: 3, origin: 6,randFactor: 0, blueprints: new int[] { 2,4,3 }), new int[]{1,2,3,4});
      sampleResult[8] = true;
      sample[9] = new WorldData(new LevelData(numOfRooms: 7, roomStyle: 4, origin: 7,randFactor: 0, blueprints: new int[] { 2,2,3,1,4,5,1 }), new int[]{2,2,2,3});
      sampleResult[9] = true;
      sample[10] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 1, origin: 3,randFactor: 0, blueprints: new int[] { 1,2,3,5 }), new int[]{1,1,1,1});
      sampleResult[10] = false;
      sample[11] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 2, origin: 4,randFactor: 0, blueprints: new int[] { 4,3,2,1 }), new int[]{4,1,2,4});
      sampleResult[11] = false;
      sample[12] = new WorldData(new LevelData(numOfRooms: 6, roomStyle: 3, origin: 5,randFactor: 0, blueprints: new int[] { 3,3,3,2,1,5 }), new int[]{4,3,2,3});
      sampleResult[12] = true;
      sample[13] = new WorldData(new LevelData(numOfRooms: 7, roomStyle: 5, origin: 9,randFactor: 0, blueprints: new int[] { 3,2,1,1,1,2,4 }), new int[]{3,2,4,4});
      sampleResult[13] = true;
      sample[14] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 3, origin: 7,randFactor: 0, blueprints: new int[] { 0,0,0,0 }), new int[]{2,3,4,4});
      sampleResult[14] = false;

      for(int i = 0; i < sample.Length; i++)
      {
         this.AddPlayerRunData(sample[i], sampleResult[i]);
      }

   }

   public void AddPlayerRunData(WorldData worldData, bool result)
   {
      this.playerRunsData.Add(worldData);
      this.playerRunsResult.Add(result);

      

      if(result == false)
      {
         deathCount++;
        this.playerDeathsData.Add(worldData);
      }
      else
      {
         surviveCount++;
      }
      Debug.Log("Added run data: " + worldData.ToString() + " Result: " + result.ToString());
      Debug.Log("Run data count: " + playerRunsData.Count.ToString() + " Death count: " + playerDeathsData.Count.ToString());
   }

   public void ClearPlayerRunData()
   {
      playerRunsData.Clear();
      playerRunsResult.Clear();
   }

  public List<WorldData> GetPlayerRunsData()
  {
      return playerRunsData;
  }
  public List<bool> GetPlayerRunsResult()
  {
      return playerRunsResult;
  }
   public List<WorldData> GetPlayerDeathsData()
   {
      return playerDeathsData;
   }

   public int GetDeathCount()
   {
      return deathCount;
   }

   public int GetSurviveCount()
   {
      return surviveCount;
   }
   


}
