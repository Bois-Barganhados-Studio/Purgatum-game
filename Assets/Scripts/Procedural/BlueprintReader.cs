using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BluePrintReader
{

    private const string FILETYPE = ".bp";
    private const string FOLDER = "Blueprints";
    private static readonly List<int> BPS = new List<int>(){0,1,2,3};
    private List<string> blueprint;
    private List<(string, Vector3)> decodedRoom; 

    public enum TileType
    {
        WALL = 0,
        GROUND = 1,
        DOOR = 2,
        CHEST = 3,
    }

    public enum BlueprintIndex
    {
        RECT = 0,
        ESPHERE = 1,
        HEXA = 2,
        CRAZY = 3,
    }

    public BluePrintReader()
    {}

    /**
     * Define um blueprint para gerar a sala
     */
    public void defineBp(int bp)
    {
        if (BPS.Contains(bp))
        {
            ReadBp(Enum.GetName(typeof(BlueprintIndex), bp));
            DecodeBp();
        }
        else
        {
            //chamar func do gustavo para gerar novo blueprint BP
            DecodeBp();
        }
    }

    /**
    * Ler iniciadores para os tilemaps
    */
    void ReadBp(string path)
    {
        TextAsset bpFile = Resources.Load<TextAsset>(FOLDER + path + FILETYPE);
        string[] lines = bpFile.text.Split('\n');
        blueprint = new List<String>();
        blueprint.AddRange(lines);
    }

    /**
     * Decode da matriz de elementos
     * custo alto visto que é uma matriz!
     */
    bool DecodeBp()
    {
        bool status = false;
        try { 
            decodedRoom =  new List<(string, Vector3)>();
            foreach (string line in blueprint)
            {
                for (int index = 0; index < line.ToCharArray().Length; index++)
                {
                    decodedRoom.Add((Enum.GetName(typeof(TileType), (int)char.GetNumericValue(line[index])),
                        SetGlobalPosition(blueprint.IndexOf(line),index)));
                }
            }
            status = true;
        }catch (Exception e){
            status = false;
            Debug.LogException(e);
        }
        return status;
    }

    /**
     * Gera um vector com a posição dos elementos dentro da sala
     */
    Vector3 SetGlobalPosition(int indexOfLine, int indexOfCol)
    {
        //fazer calculos de posicionamento e indice dentro do mapa
        return new Vector3(indexOfLine, indexOfCol, 0);
    }

    public List<(string,Vector3)> RoomReaded()
    {
        return decodedRoom;
    }

}
