using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room
{
    private List<string> blocks;
    private List<Vector3> positions;
    private List<(int, int)> maxUp, maxDown, maxLeft, maxRight;

    public Room()
    {
        blocks = new List<string>();
        positions = new List<Vector3>();
    }

    public void AddBlock(string block)
    {
        blocks.Add(block);
    }

    public void UpdateBlock(string block, int index)
    {
        blocks[index] = block;
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

    public void SetMaxUp(List<(int, int)> maxUp)
    {
        this.maxUp = maxUp;
    }

    public void SetMaxDown(List<(int, int)> maxDown)
    {
        this.maxDown = maxDown;
    }

    public void SetMaxLeft(List<(int, int)> maxLeft)
    {
        this.maxLeft = maxLeft;
    }

    public void SetMaxRight(List<(int, int)> maxRight)
    {
        this.maxRight = maxRight;
    }

    public List<(int, int)> GetMaxRight()
    {
        return maxRight;
    }

    public List<(int, int)> GetMaxLeft()
    {
        return maxLeft;
    }

    public List<(int, int)> GetMaxUp()
    {
        return maxUp;
    }

    public List<(int, int)> GetMaxDown()
    {
        return maxDown;
    }
}
