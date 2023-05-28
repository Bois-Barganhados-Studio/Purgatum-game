using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WorldData
{
    public const int MAX_ENEMY_TYPE = 3, MIN_ENEMY_TYPE = 0, NUMBER_OF_INPUTS = 6, NUMBER_OF_BLUEPRINTS = 7;
    public LevelData levelData;
    public int enemyType;


    public WorldData()
    {
        SetLevelData(levelData = new LevelData(numOfRooms: 4, roomStyle: 0, origin: 0, randFactor: 0, blueprints: new int[] { 0, 1, 2, 3 }));
        SetEnemyType(0);
    }

    public WorldData(LevelData levelData, int enemyType)
    {
        SetLevelData(levelData);
        SetEnemyType(enemyType);
    }

    public LevelData GetLevelData()
    {
        return levelData;
    }
    public void SetLevelData(LevelData levelData)
    {
        this.levelData = levelData;
    }

    public int GetEnemyType()
    {
        return enemyType;
    }
    public void SetEnemyType(int enemyType)
    {
        if (enemyType > MAX_ENEMY_TYPE)
        {
            enemyType = MAX_ENEMY_TYPE;
        }
        else if (enemyType < MIN_ENEMY_TYPE)
        {
            enemyType = MIN_ENEMY_TYPE;
        }
        
        this.enemyType = enemyType;
    }

    //Método que recebe um vetor de doubles e converte para um WorldData
    public static WorldData FromDoubleArray(double[] doubleArray)
    {
        //Arredondando os valores para inteiros
        for (int i = 0; i < doubleArray.Length; i++)
        {
            doubleArray[i] = Math.Round(doubleArray[i],0);
        }
        //Criando o LevelData
        LevelData levelData = new LevelData(numOfRooms: 4, roomStyle: 0, origin: 0, randFactor: 0, blueprints: new int[] { 0, 1, 2, 3 });
        levelData.SetNumOfRooms((int)doubleArray[0]);
        levelData.SetRoomStyle((int)doubleArray[1]);
        levelData.SetOrigin((int)doubleArray[2]);
        levelData.SetRandFactor((int)doubleArray[3]);

        //Conversão da blueprint para um vetor de inteiros
        int blueprintSize = getNumberOfDigits((int)doubleArray[4]);
        int[] blueprint = new int[blueprintSize];
        for (int i = 0; i < blueprintSize; i++)
        {
            //Reduz em 1 para começar em 0
            blueprint[i] = ((int)doubleArray[4] % 10) -1;
            doubleArray[4] /= 10;
        }

        levelData.SetBlueprints(blueprint);

        WorldData worldData = new WorldData(levelData, (int)doubleArray[5]);

        return worldData;

    }

    //Método que retorna o número de dígitos de um número inteiro
    private static int getNumberOfDigits(int number)
    {
        int digits = 0;
        while (number > 0)
        {
            number /= 10;
            digits++;
        }
        return digits;
    }

    public override string ToString()
    {
        return "LevelData: " + levelData.ToString() + ", EnemyType: " + enemyType;
    }
    
}