using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * @author Leon Jr
 */
public class ProceduralMapBuilder : MonoBehaviour
{

    private RenderLevel levelRenderer = null;
    private const string DIVIDER = "_";
    private const string FOLDER = "Maps/";
    private static readonly string[] styles = { "STONE" };
    private static List<string> stylesIni = new List<string>();
    private BluePrintReader bpReader = null;
    public int tileNumber;
    public int bp;

    public ProceduralMapBuilder()
    {
        bpReader = new BluePrintReader();
    }

    /**
     * Ler iniciadores para os tilemaps
     */
    void ReadIni(string path)
    {
        TextAsset iniFile = Resources.Load<TextAsset>(FOLDER + path);
        string[] lines = iniFile.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimEnd('\r', '\n');
        }
        stylesIni.AddRange(lines);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stylesIni.Count == 0)
        {
            foreach (string style in styles)
            {
                ReadIni(style + "/" + style.ToLower());
            }
        }
        if (BuildTerrain(0, 0, 0, bp, UnityEngine.Random.Range(1, 4)))
        {
            Debug.Log("MAPA GERADO COM SUCESSO: "+bp);
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Iniciar o novo nivel e limpar renderizador
     */
    void NewLevel()
    {
        levelRenderer.UnloadMemory();
        levelRenderer.ClearGameObject();
        if (BuildTerrain(0, 0, 0, 0, 1))
        {
            Debug.Log("MAPA GERADO COM SUCESSO");
        }
        else
        {
            Debug.Log("ERRO AO GERAR MAPA DO JOGO");
        }
    }

    /**
     * Inicia a construção de um nível da torre
     * @args roomStyle estilo da sala (STONE, GRASS, FIRE etc)
     * @args randFactor fator de aleatoriedade da construção do mapa (SEED)
     * @args numRooms número de salas que vão ser geradas nesse nivel
     * @args qual tipo de blueprint usar (quadrado, circulas, oval etc)
     */
    bool BuildTerrain(int roomStyle, int randFactor, int numRooms, int blueprint, int folderNum)
    {
        bool status = false;
        try
        {
            levelRenderer = new RenderLevel();
            List<string> chunkPaths = new List<string>();
            List<Vector3> chunkPositions = new List<Vector3>();
            bpReader.defineBp(blueprint);
            List<(string, Vector3)> structures = bpReader.RoomReaded();
            Vector3 spawnPoint = new Vector3(0,0,0);
            for (int i = 0; i < structures.Count; i++)
            {
                string structure = structures[i].Item1;
                Vector3 position = structures[i].Item2;
                string path = "";
                folderNum = folderNum<3? folderNum+1 : 1;
                if (structure.Equals("SPAWN"))
                {
                    path = structure + DIVIDER + styles[roomStyle];
                    spawnPoint = position;
                }
                else
                {
                    path = structure + DIVIDER + styles[roomStyle] + DIVIDER + folderNum;
                }
                if (stylesIni.Contains(path))
                {
                    chunkPaths.Add(FOLDER + styles[roomStyle] + "/" + path);
                    chunkPositions.Add(position);
                }
            }
            levelRenderer.AddChunks(chunkPaths, chunkPositions);
            GameObject.Find("Character").transform.position = spawnPoint;
            levelRenderer.RenderElements();
            status = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return status;
    }
}
