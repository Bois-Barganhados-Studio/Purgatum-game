using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{

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