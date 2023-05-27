using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private const int MAX_ROOMS = 16, MIN_ROOMS = 2, SZ_ROOM_STYLE = 5, MAX_ORIGIN = 16, MIN_ORIGIN = 0, MAX_RAND_FACTOR = 20, MIN_RAND_FACTOR = 0;
    private int numOfRooms, roomStyle, origin, randFactor;
    private int[] blueprints;
    public LevelData(int numOfRooms, int roomStyle, int origin, int randFactor, int[] blueprints)
    {
        SetNumOfRooms(numOfRooms);
        SetRoomStyle(roomStyle);
        SetOrigin(origin);
        SetRandFactor(randFactor);
        SetBlueprints(blueprints);
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
        if (numOfRooms > MAX_ROOMS)
        {
            numOfRooms = MAX_ROOMS;
        }
        else if (numOfRooms < MIN_ROOMS)
        {
            numOfRooms = MIN_ROOMS;
        }
        else
        {
            this.numOfRooms = numOfRooms;
        }
    }

    public void SetRoomStyle(int roomStyle)
    {
        if (roomStyle > SZ_ROOM_STYLE)
        {
            roomStyle = SZ_ROOM_STYLE - 1;
        }
        else if (roomStyle < 0)
        {
            roomStyle = 0;
        }
        else
        {
            this.roomStyle = roomStyle;
        }
    }

    public void SetOrigin(int origin)
    {
        if (origin > MAX_ORIGIN)
        {
            origin = MAX_ORIGIN;
        }
        else if (origin < MIN_ORIGIN)
        {
            origin = MIN_ORIGIN;
        }
        else
        {
            this.origin = origin;
        }
    }

    public void SetRandFactor(int randFactor)
    {
        if (randFactor < MIN_RAND_FACTOR)
        {
            randFactor = MIN_RAND_FACTOR;
        }
        else if (randFactor > MAX_RAND_FACTOR)
        {
            randFactor = MAX_RAND_FACTOR;
        }
        else
        {
            this.randFactor = randFactor;
        }
    }

    public void SetBlueprints(int[] blueprints)
    {
        int[] filtredBlueprints = new int[GetNumOfRooms()];
        int diff = numOfRooms - blueprints.Length;
        if (blueprints.Length < numOfRooms)
        {
            for (int i = 0; i < diff; i++)
            {
                filtredBlueprints[i] = Random.Range(0, BluePrintReader.BPS.Length - 1);
            }
        }
        for (int i = blueprints.Length - diff, bp = 0; i < blueprints.Length; i++, bp = blueprints[i])
        {
            if (bp < 0)
            {
                filtredBlueprints[i] = 0;
            }
            else if (bp > BluePrintReader.BPS.Length - 1)
            {
                filtredBlueprints[i] = BluePrintReader.BPS.Length - 1;
            }
            else if (!System.Array.Exists(BluePrintReader.BPS, element => element == bp))
            {
                filtredBlueprints[i] = 0;
            }
            else
            {
                filtredBlueprints[i] = blueprints[i];
            }
        }
        this.blueprints = filtredBlueprints;
    }
}