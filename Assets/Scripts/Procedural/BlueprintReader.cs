using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BluePrintReader
{
    private const string FOLDER = "Blueprints/";
    private static readonly int[] BPS = { 0, 1, 2, 3, 4 };
    private static readonly List<TextAsset> BPS_DATA = new List<TextAsset>();
    private List<string> blueprint = new List<string>();
    private Room decodedRoom;
    public const int SIZE_OF_CHUNK = 10;
    public const double W_PIXELS = 3.2;
    public const double H_PIXELS = 3.2;
    private int numOfRooms = 0;
    private List<Room> rooms = new List<Room>();

    public void SetNumOfRooms(int numOfRooms)
    {
        rooms.Clear();
        this.numOfRooms = numOfRooms;
    }

    public enum TileType
    {
        WALL = 'w',
        GROUND = 'g',
        CHEST = 'c',
        SPAWN = 's',
        MSPAWN = 'm',
    }

    public enum BlueprintIndex
    {
        RECT = 0,
        CIRCLE = 1,
        HEXA = 2,
        CRAZY = 3,
        CRUX = 4,
    }

    public BluePrintReader()
    { }

    /**
     * Define um blueprint para gerar a sala
     */
    public void defineBp(int[] bp)
    {
        foreach (int bpItem in bp)
        {
            if (bpItem < BPS.Length)
            {
                if (BPS_DATA.Count == 0)
                {
                    ReadBpFromDisk();
                }
                blueprint.Clear();
                string[] lines = BPS_DATA[bpItem].text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].TrimEnd('\r', '\n');
                }
                blueprint.AddRange(lines);
                DecodeBp();
            }
            else
            {
                //chamar func do gustavo para gerar novo blueprint BP
                DecodeBp();
            }
        }
    }

    /**
    * Ler iniciadores para os tilemaps
    */
    void ReadBpFromDisk()
    {
        foreach (int bp in BPS)
        {
            string path = Enum.GetName(typeof(BlueprintIndex), bp);
            TextAsset bpFile = Resources.Load<TextAsset>(FOLDER + path);
            BPS_DATA.Add(bpFile);
        }
    }

    /**
     * Decode da matriz de elementos
     * custo alto visto que é uma matriz!
     */
    bool DecodeBp()
    {
        bool status = false;
        try
        {
            decodedRoom = new Room();
            for (int lineIndex = 0; lineIndex < blueprint.Count; lineIndex++)
            {
                string line = blueprint[lineIndex];
                for (int colIndex = 0; colIndex < line.ToCharArray().Length; colIndex++)
                {
                    char letter = line[colIndex];
                    if (letter != ' ')
                    {
                        decodedRoom.AddBlock((Enum.GetName(typeof(TileType), letter)));
                        decodedRoom.AddPosition(SetGlobalPosition(lineIndex, colIndex));   
                    }
                }
            }
            rooms.Add(decodedRoom);
            status = true;
        }
        catch (Exception e)
        {
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
        (double, double) tuple = ToIsometric(indexOfLine, indexOfCol);
        return new Vector3((float)tuple.Item1, (float)tuple.Item2, 0);
    }

    public List<Room> RoomsLoaded()
    {
        return rooms;
    }

    /**
     * Convertendo Coordenadas
     */
    private (double, double) ToIsometric(double x, double y)
    {
        double nx = (x * 0.5 * (W_PIXELS)) + (y * -0.5 * (W_PIXELS));
        double ny = (x * 0.25 * (H_PIXELS)) + (y * 0.25 * (H_PIXELS));
        return (nx, ny);
    }

}
