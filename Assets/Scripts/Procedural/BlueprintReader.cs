using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BluePrintReader
{
    //constantes
    private const string FOLDER = "Blueprints/";
    public const int SIZE_OF_CHUNK = 10;
    public static readonly int SIZE_OF_ROOM = 20;
    public const double W_PIXELS = 3.2;
    public const double H_PIXELS = 3.2;
    private static readonly List<char> WALLS = new List<char> { (char)TileType.WALL_NW, (char)TileType.WALL_NE, (char)TileType.WALL_SW, (char)TileType.WALL_SE, (char)TileType.WALL_N, (char)TileType.WALL_E, (char)TileType.WALL_S, (char)TileType.WALL_W };
    public static readonly int BLUEPRINTS_TOTAL = 5;
    public static readonly int[] BPS = { 0, 1, 2, 3, 4, 5, 6 };
    private static readonly List<TextAsset> BPS_DATA = new List<TextAsset>();
    //utils
    private List<string> blueprint = new List<string>();
    private List<Room> rooms = new List<Room>();
    private Room decodedRoom;
    private int numOfRooms = 0;

    public enum TileType
    {
        GROUND = 'g',
        WALL_NW = ']',
        WALL_NE = '=',
        WALL_SW = '-',
        WALL_SE = '[',
        WALL_N = 'C',
        WALL_E = 'D',
        WALL_S = 'B',
        WALL_W = 'E',
        CHEST = 'c',
        SPAWN = 's',
    }

    public enum BlueprintIndex
    {
        RECT = 0,
        CIRCLE = 1,
        HEXA = 2,
        CRAZY = 3,
        CRUX = 4,
        WAYS = 5,
        EGG = 6,
    }

    public BluePrintReader()
    { }

    public void SetNumOfRooms(int numOfRooms)
    {
        rooms.Clear();
        this.numOfRooms = numOfRooms;
    }

    /**
     * Define um blueprint para gerar a sala
     */
    public void DefineBp(int[] bp)
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
        }
    }

    /**
    * Ler iniciadores para os tilemaps
    */
    public void ReadBpFromDisk()
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
    public bool DecodeBp()
    {
        bool status = false;
        int insertIndex = 0;
        List<(int, int)> maxUp = new List<(int, int)>();
        List<(int, int)> maxDown = new List<(int, int)>();
        List<(int, int)> maxLeft = new List<(int, int)>();
        List<(int, int)> maxRight = new List<(int, int)>();
        try
        {
            //fazer codigo do DefineMaxIndexes aqui integrado
            decodedRoom = new Room();
            for (int lineIndex = 0; lineIndex < blueprint.Count; lineIndex++)
            {
                string line = blueprint[lineIndex];
                string postline = "";
                if (lineIndex == 0)
                {
                    postline = blueprint[1];
                }
                else if (lineIndex == SIZE_OF_ROOM - 1)
                {
                    postline = blueprint[SIZE_OF_ROOM - 2];
                }
                for (int colIndex = 0; colIndex < line.Length; colIndex++)
                {
                    char letter = line[colIndex];
                    if (lineIndex == 0)
                    {
                        if (WALLS.Contains(letter) && colIndex < postline.Length && postline[colIndex] == ((char)TileType.GROUND))
                        {
                            maxUp.Add((insertIndex, colIndex));
                        }
                    }
                    if (lineIndex == SIZE_OF_ROOM - 1)
                    {
                        if (WALLS.Contains(letter) && colIndex < postline.Length && postline[colIndex] == ((char)TileType.GROUND))
                        {
                            maxDown.Add((insertIndex, colIndex));
                        }
                    }
                    if (colIndex == 0)
                    {
                        if (WALLS.Contains(letter) && colIndex + 1 < line.Length && line[colIndex + 1] == ((char)TileType.GROUND))
                        {
                            maxLeft.Add((insertIndex, lineIndex));
                        }
                    }
                    if (colIndex == SIZE_OF_ROOM - 1)
                    {
                        if (WALLS.Contains(letter) && line[colIndex - 1] == ((char)TileType.GROUND))
                        {
                            maxRight.Add((insertIndex, lineIndex));
                        }
                    }
                    if (letter != ' ')
                    {
                        insertIndex++;
                        decodedRoom.AddBlock((Enum.GetName(typeof(TileType), letter)));
                        decodedRoom.AddPosition(SetGlobalPosition(lineIndex, colIndex));
                    }
                }
            }
            decodedRoom.SetMaxUp(maxUp);
            decodedRoom.SetMaxDown(maxDown);
            decodedRoom.SetMaxLeft(maxLeft);
            decodedRoom.SetMaxRight(maxRight);
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
    private Vector3 SetGlobalPosition(int indexOfLine, int indexOfCol)
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
    public (double, double) ToIsometric(double x, double y)
    {
        double nx = (x * 0.5 * (W_PIXELS)) + (y * -0.5 * (W_PIXELS));
        double ny = (x * 0.25 * (H_PIXELS)) + (y * 0.25 * (H_PIXELS));
        return (nx, ny);
    }

}
