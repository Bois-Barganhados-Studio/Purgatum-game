using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WorldData
{
    public const int MAX_ENEMY_TYPE = 3, MIN_ENEMY_TYPE = 0, NUMBER_OF_INPUTS = 6, NUMBER_OF_BLUEPRINTS = 7, QTD_OF_ENEMYS = 4;

    public LevelData levelData;
    public int[] enemyType;


    public WorldData()
    {
        SetLevelData(levelData = new LevelData(numOfRooms: 4, roomStyle: 0, origin: 0, randFactor: 0, blueprints: new int[] { 0, 1, 2, 3 }));
        SetEnemyType(new int[] { 0, 0, 0, 0 });
    }

    public WorldData(LevelData levelData, int[] enemyType)
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

    public int[] GetEnemyType()
    {
        return enemyType;
    }
    public void SetEnemyType(int[] enemyType)
    {
        //Fitra os valores para o intervalo permitido
        for(int i = 0; i < enemyType.Length; i++)
        {
            enemyType[i] = enemyType[i] % QTD_OF_ENEMYS;
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

        int[] enemyType = new int[QTD_OF_ENEMYS];
        for (int i = 0; i < QTD_OF_ENEMYS; i++)
        {
           enemyType[i] = ((int)doubleArray[5] % 10) -1;
            doubleArray[5] /= 10;

        }

        WorldData worldData = new WorldData(levelData, enemyType);

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
        string s = "LevelData: " + levelData.ToString() + ", EnemyType: ";
        for(int i = 0; i < enemyType.Length; i++)
        {
            s += enemyType[i] + ", ";
        }
        return s;
    }
    
    public static WorldData Default()
    {
        return new WorldData();
    }


}