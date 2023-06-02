using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    private const int NUMBER_OF_INPUTS = 6, MAX_GENERATIONS_ATTEMPTS = 5;

    #region Banco de dados
    public List<WorldData> playerRunsData = new List<WorldData>();
    public List<bool> playerRunsResult = new List<bool>();
    public List<WorldData> playerDeathsData = new List<WorldData>();

    //******************************************************************//

    public double[][] playerRunsDataArray = null;
    public double[][] playerRunsResultArray = null;
    public double[][] playerDeathsDataArray = null;
    #endregion

    private NeuralNetwork neuralNetwork = null;

    public AIController()
    {
        neuralNetwork = new NeuralNetwork(new int[] { NUMBER_OF_INPUTS, 10, 10, 10, 1 }, epochs: 10000, randomSeed: -1, learningRate: 1.5, bias: 1, tolerate: 0.01, maxAttempts: 5);

        playerRunsData = new List<WorldData>();
        playerRunsResult = new List<bool>();
        playerDeathsData = new List<WorldData>();
    }

    //Pra testar
    private void Start()
    {

        neuralNetwork = new NeuralNetwork(new int[] { NUMBER_OF_INPUTS, 10, 10, 1 }, epochs: 1000, randomSeed: -1, learningRate: 0.01, bias: 1, tolerate: 0.0001, maxAttempts: 5);

        playerRunsData = new List<WorldData>();
        playerRunsResult = new List<bool>();
        playerDeathsData = new List<WorldData>();

        #region Remover pois os dados serão carregados do roguelogic
        WorldData[] sample = new WorldData[15];
        bool[] sampleResult = new bool[15];


        #region Cria os dados de teste
        sample[0] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 0, origin: 4, randFactor: 1, blueprints: new int[] { 0, 1, 2, 3 }), new int[] { 0, 1, 1, 3 });
        sampleResult[0] = false;
        sample[1] = new WorldData(new LevelData(numOfRooms: 5, roomStyle: 1, origin: 3, randFactor: 7, blueprints: new int[] { 4, 3, 2, 1, 5 }), new int[] { 4, 2, 0, 1 });
        sampleResult[1] = true;
        sample[2] = new WorldData(new LevelData(numOfRooms: 8, roomStyle: 2, origin: 3, randFactor: 2, blueprints: new int[] { 1, 1, 4, 3, 2, 3, 5, 1 }), new int[] { 4, 1, 1, 3 });
        sampleResult[2] = false;
        sample[3] = new WorldData(new LevelData(numOfRooms: 3, roomStyle: 3, origin: 3, randFactor: 5, blueprints: new int[] { 0, 1, 0 }), new int[] { 0, 1, 0, 0 });
        sampleResult[3] = true;
        sample[4] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 4, origin: 2, randFactor: 20, blueprints: new int[] { 1, 4, 2, 0 }), new int[] { 2, 3, 1, 1 });
        sampleResult[4] = false;
        sample[5] = new WorldData(new LevelData(numOfRooms: 5, roomStyle: 5, origin: 1, randFactor: 19, blueprints: new int[] { 0, 1, 0, 1, 0 }), new int[] { 1, 2, 1, 1 });
        sampleResult[5] = false;
        sample[6] = new WorldData(new LevelData(numOfRooms: 9, roomStyle: 2, origin: 4, randFactor: 18, blueprints: new int[] { 0, 2, 3, 4, 1, 0, 5, 1, 2 }), new int[] { 1, 2, 2, 3 });
        sampleResult[6] = false;
        sample[7] = new WorldData(new LevelData(numOfRooms: 2, roomStyle: 2, origin: 5, randFactor: 11, blueprints: new int[] { 0, 5 }), new int[] { 2, 3, 4, 3 });
        sampleResult[7] = true;
        sample[8] = new WorldData(new LevelData(numOfRooms: 3, roomStyle: 3, origin: 6, randFactor: 9, blueprints: new int[] { 2, 4, 3 }), new int[] { 1, 2, 3, 4 });
        sampleResult[8] = true;
        sample[9] = new WorldData(new LevelData(numOfRooms: 7, roomStyle: 4, origin: 7, randFactor: 15, blueprints: new int[] { 2, 2, 3, 1, 4, 5, 1 }), new int[] { 2, 2, 2, 3 });
        sampleResult[9] = true;
        sample[10] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 1, origin: 3, randFactor: 1, blueprints: new int[] { 1, 2, 3, 5 }), new int[] { 1, 1, 1, 1 });
        sampleResult[10] = false;
        sample[11] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 2, origin: 4, randFactor: 19, blueprints: new int[] { 4, 3, 2, 1 }), new int[] { 4, 1, 2, 4 });
        sampleResult[11] = false;
        sample[12] = new WorldData(new LevelData(numOfRooms: 6, roomStyle: 3, origin: 5, randFactor: 8, blueprints: new int[] { 3, 3, 3, 2, 1, 5 }), new int[] { 4, 3, 2, 3 });
        sampleResult[12] = true;
        sample[13] = new WorldData(new LevelData(numOfRooms: 7, roomStyle: 5, origin: 9, randFactor: 10, blueprints: new int[] { 3, 2, 1, 1, 1, 2, 4 }), new int[] { 3, 2, 4, 4 });
        sampleResult[13] = true;
        sample[14] = new WorldData(new LevelData(numOfRooms: 4, roomStyle: 3, origin: 7, randFactor: 20, blueprints: new int[] { 0, 0, 0, 0 }), new int[] { 2, 3, 4, 4 });
        sampleResult[14] = false;

        #endregion


        for (int i = 0; i < sample.Length; i++)
        {
            this.AddPlayerRunData(sample[i], sampleResult[i]);
        }


        this.convertPlayerRunsData();
        this.convertPlayerRunsResult();
        this.convertPlayerDeathsData();

        //Mostra os dados originais e convertidos


        Debug.Log(this.playerRunsData[0]);
        string s = "PlayerRunData: ";

        for (int j = 0; j < this.playerRunsDataArray[0].Length; j++)
        {
            s += this.playerRunsDataArray[0][j] + " ";
        }
        s += " PlayerRunResult: ";
        for (int j = 0; j < this.playerRunsResultArray[0].Length; j++)
        {
            s += this.playerRunsResultArray[0][j] + " ";
        }
        Debug.Log(s);


        double[][] normalized = this.normalizeData(this.playerRunsDataArray);
        s = "normalized: ";

        for (int j = 0; j < normalized[0].Length; j++)
        {
            s += normalized[0][j] + " ";
        }

        Debug.Log(s);

        #endregion

    }

    //Teste, deve estar no roguedata
    private void AddPlayerRunData(WorldData worldData, bool result)
    {
        this.playerRunsData.Add(worldData);
        this.playerRunsResult.Add(result);

        if (result == false)
        {
            this.playerDeathsData.Add(worldData);
        }
    }

    public void SetData(RogueData rd)
    {
        this.playerRunsData = rd.playerRunsData;
        this.playerRunsResult = rd.playerRunsResult;
        this.playerDeathsData = rd.playerDeathsData;

        this.convertPlayerRunsData();
        this.convertPlayerRunsResult();
        this.convertPlayerDeathsData();

        Debug.Log("PlayerRunsData: " + playerRunsData);
        Debug.Log("PlayerRunsResult: " + playerRunsResult);
    }

    #region ConvertData
    //Converte os dados de criação de um mundo para um dados em double
    private double[] convertWorldDataToDouble(WorldData worldData)
    {
        double[] worldDataDouble = new double[NUMBER_OF_INPUTS];
        worldDataDouble = new double[NUMBER_OF_INPUTS];

        //Converte o numero de salas
        worldDataDouble[0] = (double)(worldData.levelData.GetNumOfRooms());
        //Converte o estilo das salas
        worldDataDouble[1] = (double)(worldData.levelData.GetRoomStyle());
        //Converte a origem das salas
        worldDataDouble[2] = (double)(worldData.levelData.GetOrigin());
        //Converte o fator de aleatoriedade
        worldDataDouble[3] = (double)(worldData.levelData.GetRandFactor());
        //Converte a lista de blueprints
        worldDataDouble[4] = this.intArrayToDouble(worldData.levelData.GetBlueprints());
        //Converte o tipo de inimigo
        worldDataDouble[5] = this.intArrayToDouble(worldData.enemyType);

        return worldDataDouble;
    }

    //Converte a lista de blueprints para um dado em double
    private double intArrayToDouble(int[] intArray)
    {
        double arrayUnificado = 0;
        for (int i = intArray.Length - 1; i >= 0; i--)
        {
            arrayUnificado += (1 + (double)(intArray[i])) * MathF.Pow(10, intArray.Length - i - 1);
        }
        arrayUnificado /= MathF.Pow(10, intArray.Length);
        return arrayUnificado;
    }


    //Converte os dados de criação do mundo para um dados em double
    private void convertPlayerRunsData()
    {
        this.playerRunsDataArray = new double[playerRunsData.Count][];
        for (int i = 0; i < playerRunsDataArray.Length; i++)
        {
            this.playerRunsDataArray[i] = this.convertWorldDataToDouble(playerRunsData[i]);
        }
    }

    //Converte os resultados de criação do mundo para um dados em double
    private void convertPlayerRunsResult()
    {
        this.playerRunsResultArray = new double[playerRunsResult.Count][];
        for (int i = 0; i < playerRunsResultArray.Length; i++)
        {
            this.playerRunsResultArray[i] = new double[1];
            if (playerRunsResult[i])
                this.playerRunsResultArray[i][0] = 1.0;
            else
                this.playerRunsResultArray[i][0] = 0.0;
        }

    }

    //Converte os dados do mundo da morte do jogador para um dados em double
    private void convertPlayerDeathsData()
    {
        this.playerDeathsDataArray = new double[playerDeathsData.Count][];
        for (int i = 0; i < playerDeathsDataArray.Length; i++)
        {
            this.playerDeathsDataArray[i] = this.convertWorldDataToDouble(playerDeathsData[i]);
        }

    }

    #endregion

    #region Train

    private double[][] normalizeData(double[][] data)
    {
        double[][] normalizedData = new double[data.Length][];
        double[] min = new double[data[0].Length]; //Vetor de minimos para cada coluna
        double[] max = new double[data[0].Length]; //Vetor de maximos para cada coluna
                                                   //Itera sobre cada coluna, pegando o minimo e o maximo

        for (int i = 0; i < data[0].Length; i++)
        {
            max[i] = Double.MinValue;
            min[i] = Double.MaxValue;
        }

        for (int i = 0; i < data.Length; i++)
        {
            // max[i] = Double.MinValue;
            // min[i] = Double.MaxValue;
            for (int j = 0; j < data[i].Length; j++)
            {
                if (data[i][j] < min[j])
                {
                    min[j] = data[i][j];
                }

                if (data[i][j] > max[j])
                {
                    max[j] = data[i][j];
                }

            }
        }

        //Imprime os valores de min e max de cada coluna
        for (int i = 0; i < data[0].Length; i++)
        {
            //Debug.Log("Col: " + i + "Min: " + min[i] + " Max: " + max[i]);
        }

        //Itera sobre cada coluna, normalizando os dados
        for (int i = 0; i < data.Length; i++)
        {
            normalizedData[i] = new double[data[i].Length];
            for (int j = 0; j < data[i].Length; j++)
            {
                double div = (max[j] - min[j]);
                div = div == 0 ? 1 : div;
                normalizedData[i][j] = (data[i][j] - min[j]) / div;
            }

        }
        return normalizedData;
    }

    public void Train()
    {
        double[][] output = new double[playerRunsResultArray.Length][];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = new double[1];
            output[i][0] = playerRunsResultArray[i][0];
        }

        double[][] input = this.normalizeData(playerRunsDataArray);

        neuralNetwork.train(input, output, log: false);
    }

    public void TestAI()
    {
        double[][] output = new double[playerRunsResultArray.Length][];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = new double[1];
            output[i][0] = playerRunsResultArray[i][0];
        }

        double[][] input = this.normalizeData(playerRunsDataArray);

        double[][] predicted = new double[input.Length][];
        for (int i = 0; i < predicted.Length; i++)
        {
            predicted[i] = new double[1];
            predicted[i][0] = neuralNetwork.FeedFoward(input[i])[0];
        }

        for (int i = 0; i < predicted.Length; i++)
        {
            string s = "Input: ";
            for (int j = 0; j < input[i].Length; j++)
            {
                s += input[i][j] + " ";
            }
            s += "Output: " + Math.Round(predicted[i][0], 10) + " Expected: " + output[i][0];
            Debug.Log(s);
        }


    }

    #endregion

    #region GenerateParams


    public WorldData GenerateRandomParams()
    {
        LevelData levelData = new LevelData(numOfRooms: 4, roomStyle: 0, origin: 0, randFactor: 0, blueprints: new int[] { 0, 1, 2, 3 });

        levelData.SetNumOfRooms(UnityEngine.Random.Range(2, 17));
        levelData.SetRoomStyle(UnityEngine.Random.Range(0, 3));
        levelData.SetOrigin(UnityEngine.Random.Range(0, 16));
        levelData.SetRandFactor(UnityEngine.Random.Range(0, 21));
        int[] blueprints = new int[levelData.GetNumOfRooms()];
        for (int i = 0; i < levelData.GetNumOfRooms(); i++)
        {
            blueprints[i] = UnityEngine.Random.Range(0, 7);
        }
        levelData.SetBlueprints(blueprints);
        System.Console.Write("Params = NumOfRooms: " + levelData.GetNumOfRooms() + ", RoomStyle: " + levelData.GetRoomStyle() + ", Origin: " + levelData.GetOrigin() + ", RandFactor: " + levelData.GetRandFactor() + ", Blueprints: " + string.Join(", ", blueprints));

        int[] enemyType = new int[WorldData.QTD_OF_ENEMYS];
        for (int i = 0; i < enemyType.Length; i++)
        {
            enemyType[i] = UnityEngine.Random.Range(WorldData.MIN_ENEMY_TYPE, WorldData.MAX_ENEMY_TYPE + 1);
        }

        WorldData worldData = new WorldData(levelData, enemyType);
        worldData = this.FilterWorldData(worldData);
        return worldData;
    }

    public WorldData GenerateWorldParams()
    {
        WorldData worldData = new WorldData();
        int attempt = 0;

        while (attempt < MAX_GENERATIONS_ATTEMPTS)
        {
            shuffle(playerDeathsDataArray);
            worldData = this.GetNewWorldParams();
            double[] worldDataArray = this.convertWorldDataToDouble(worldData);
            double[][] minMax = new double[worldDataArray.Length][];
            for (int i = 0; i < minMax.Length; i++)
            {
                minMax[i] = this.getMinMax(playerRunsDataArray, i);
            }

            //Normaliza o worldDataArray de acordo com o min e max de cada coluna do banco de Runs
            double[] normalizedWorldDataArray = new double[worldDataArray.Length];
            for (int i = 0; i < worldDataArray.Length; i++)
            {
                double div = (minMax[i][1] - minMax[i][0]);
                div = div == 0 ? 1 : div;
                normalizedWorldDataArray[i] = (worldDataArray[i] - minMax[i][0]) / div;
            }

            double result = neuralNetwork.FeedFoward(worldDataArray)[0];
            //Se menor que 0.5, prevê que o player deve morrer
            if (result < 0.5)
            {
                Debug.Log("Player should run: " + result + " attempt: " + attempt);
                attempt = MAX_GENERATIONS_ATTEMPTS;
            }
            else
            {
                if (attempt == MAX_GENERATIONS_ATTEMPTS - 1)
                {
                    Debug.Log("World randomly generated :(");
                    worldData = this.GenerateRandomParams();
                }
            }
        }

        worldData = this.FilterWorldData(worldData);

        return worldData;
    }

    private WorldData FilterWorldData(WorldData worldData)
    {
        //Filtra Room Style valor 1 do array
        if (worldData.GetLevelData().GetRoomStyle() < WorldData.MIN_ROOM_STYLE)
        {
            worldData.GetLevelData().SetRoomStyle(WorldData.MIN_ROOM_STYLE);
        }
        else if (worldData.GetLevelData().GetRoomStyle() > WorldData.MAX_ROOM_STYLE)
        {
            worldData.GetLevelData().SetRoomStyle(WorldData.MAX_ROOM_STYLE);
        }

        //Filtra o Rand Factor
        if(worldData.GetLevelData().GetRandFactor() > LevelData.MAX_RAND_FACTOR)
        {
            worldData.GetLevelData().SetRandFactor(LevelData.MAX_RAND_FACTOR);
        }
        else if (worldData.GetLevelData().GetRandFactor() < LevelData.MIN_RAND_FACTOR)
        {
            worldData.GetLevelData().SetRandFactor(LevelData.MIN_RAND_FACTOR);
        }


        //Filtra os tipos de inimigos
        int[] enemyType = worldData.GetEnemyType();
        for (int i = 0; i < enemyType.Length; i++)
        {
            if (enemyType[i] < WorldData.MIN_ENEMY_TYPE)
            {
                enemyType[i] = WorldData.MIN_ENEMY_TYPE;
            }
            else if (enemyType[i] > WorldData.MAX_ENEMY_TYPE)
            {
                enemyType[i] = WorldData.MAX_ENEMY_TYPE;
            }
        }


        //Filtra as blueprints
        int[] blueprints = worldData.GetLevelData().GetBlueprints();
        for (int i = 0; i < blueprints.Length; i++)
        {
            blueprints[i] = blueprints[i] % WorldData.NUMBER_OF_BLUEPRINTS;
        }
        worldData.GetLevelData().SetBlueprints(blueprints);

        return worldData;
    }

    public WorldData GetNewWorldParams()
    {
        WorldData worldData = new WorldData();
        LevelData levelData = new LevelData(1, 1, 1, 1, new int[] { 1 });
        levelData.SetNumOfRooms((int)this.getMean(playerDeathsDataArray, 0));
        levelData.SetRoomStyle((int)this.getMode(playerDeathsDataArray, 1));
        levelData.SetOrigin((int)this.getMean(playerDeathsDataArray, 2));
        levelData.SetRandFactor((int)this.getMean(playerDeathsDataArray, 3));
        levelData.SetBlueprints(this.desconvertBlueprint(this.getMean(playerDeathsDataArray, 4), levelData.GetNumOfRooms()));
        worldData.SetLevelData(levelData);
        worldData.SetEnemyType(this.desconvertEnemyType(this.getMean(playerDeathsDataArray, 5)));

        return worldData;
    }

    private void shuffle(double[][] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, data.Length);
            double[] temp = data[i];
            data[i] = data[randomIndex];
            data[randomIndex] = temp;
        }
    }

    private double[] getMinMax(double[][] data, int index)
    {
        double[] minMax = new double[2];
        minMax[0] = double.MaxValue;
        minMax[1] = double.MinValue;
        for (int i = 0; i < data.Length; i++)
        {

            if (data[i][index] < minMax[0])
            {
                minMax[0] = data[i][index];
            }
            if (data[i][index] > minMax[1])
            {
                minMax[1] = data[i][index];
            }
        }
        Debug.Log("minmax para " + index + " = " + minMax[0] + " | " + minMax[1]);
        return minMax;
    }

    private double getMean(double[][] data, int index, double modifier = 1)
    {
        double sum = 0;
        double indexSum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i][index] * ((i + 1) * modifier);
            indexSum += ((i + 1) * modifier);
        }
        return (sum / indexSum);
    }

    private double getMode(double[][] data, int index, double modifier = 1)
    {
        Dictionary<double, double> dict = new Dictionary<double, double>();
        for (int i = 0; i < data.Length; i++)
        {
            if (dict.ContainsKey(data[i][index]))
            {
                dict[data[i][index]] += (i + 1) * modifier;
            }
            else
            {
                dict.Add(data[i][index], (i + 1) * modifier);
            }
        }

        double mode = 0;
        double max = 0;
        foreach (KeyValuePair<double, double> pair in dict)
        {
            if (pair.Value > max)
            {
                max = pair.Value;
                mode = pair.Key;
            }
        }
        return mode;
    }

    private int[] desconvertBlueprint(double blueprint, int numOfRooms)
    {
        int[] blueprintArray = new int[numOfRooms];
        blueprint = (double)MathF.Round((float)blueprint, numOfRooms);
        for (int i = 0; i < numOfRooms; i++)
        {
            blueprint = (blueprint * 10) % (WorldData.NUMBER_OF_BLUEPRINTS);
            blueprintArray[i] = (int)blueprint;
            blueprint = blueprint - ((int)blueprint);       //pega a parte decimal
        }
        return blueprintArray;
    }

    private int[] desconvertEnemyType(double enemyType)
    {
        int[] enemyTypeArray = new int[WorldData.QTD_OF_ENEMYS];
        enemyType = (double)MathF.Round((float)enemyType, WorldData.QTD_OF_ENEMYS);
        for (int i = 0; i < WorldData.QTD_OF_ENEMYS; i++)
        {
            enemyType = (enemyType * 10) % (WorldData.QTD_OF_ENEMYS);
            enemyTypeArray[i] = (int)enemyType;
            enemyType = enemyType - ((int)enemyType);       //pega a parte decimal
        }
        return enemyTypeArray;
    }

    #endregion

}