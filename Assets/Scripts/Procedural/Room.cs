using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{

    private List<string> blocks;
    private List<Vector3> positions;

    public Room() {
        blocks = new List<string>();
        positions = new List<Vector3>();
    }

    public void AddBlock(string block)
    {
        blocks.Add(block);
    }

    public void AddPosition(Vector3 position)
    {
        positions.Add(position);
    }

    public void UpdatePositions(List<Vector3> positions)
    {
        this.positions = positions;
    }

    public int SizeOf()
    {
        return blocks.Count;
    }

    public string GetBlock(int pos)
    {
        return blocks[pos];
    }

    public Vector3 GetPosition(int pos)
    {
        return positions[pos];
    }

    public List<string> GetBlocks()
    {
        return blocks;
    }

    public List<Vector3> GetPositions()
    {
        return positions;
    }
}
