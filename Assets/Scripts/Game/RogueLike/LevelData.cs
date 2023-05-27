using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private const int MAX_ROOMS = 16;
    private const int MIN_ROOMS = 2;
    private const int SZ_ROOM_STYLE = 5;
    private const int MAX_ORIGIN = 16;
    private const int MIN_ORIGIN = 0;
    private int numOfRooms, roomStyle, origin, randFactor;
    private int[] blueprints;
    public LevelData(int numOfRooms, int roomStyle, int origin, int randFactor, int[] blueprints)
    {
        this.numOfRooms = numOfRooms;
        this.roomStyle = roomStyle;
        this.origin = origin;
        this.randFactor = randFactor;
        this.blueprints = blueprints;
    }

    public int GetNumOfRooms()
    {
        return numOfRooms;
    }

    public int GetRoomStyle()
    {
        return roomStyle;
    }

    public int GetOrigin()
    {
        return origin;
    }

    public int GetRandFactor()
    {
        return randFactor;
    }

    public int[] GetBlueprints()
    {
        return blueprints;
    }

    public void SetNumOfRooms(int numOfRooms)
    {
        this.numOfRooms = numOfRooms;
    }

    public void SetRoomStyle(int roomStyle)
    {
        this.roomStyle = roomStyle;
    }

    public void SetOrigin(int origin)
    {
        this.origin = origin;
    }

    public void SetRandFactor(int randFactor)
    {
        this.randFactor = randFactor;
    }

    public void SetBlueprints(int[] blueprints)
    {
        this.blueprints = blueprints;
    }
}