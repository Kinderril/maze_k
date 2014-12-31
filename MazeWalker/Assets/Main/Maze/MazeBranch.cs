﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public struct Direction
{
    public IntPos intPos;
    public Side side;

    public Direction(IntPos p, Side s)
    {
        intPos = p;
        side = s;
    }
}

public enum Side
{
    up,
    down,
    left,
    right
}

public class MazeBranch
{
    private GridInfo[,] grid;
    private Direction[] directions = new Direction[4] { new Direction(new IntPos(0, 1), Side.up), new Direction(new IntPos(1, 0), Side.right), new Direction(new IntPos(0, -1),Side.down), new Direction(new IntPos(-1, 0),Side.left) };
    private System.Random random;
    private int size;
    private List<GridInfo> history = new List<GridInfo>();
    private int brachLevel;
    private int childBranchNumber = 0;
    private MazeController mazeController;


    public MazeBranch(GridInfo[,] grid, System.Random random, int size, int prevBranchLevel, MazeController mazeController)
    {
        this.mazeController = mazeController;
        this.grid = grid;
        this.random = random;
        this.size = size;
        brachLevel = prevBranchLevel + 1;
    }

    public void DoBranch(GridInfo curPos)
    {
        history.Add(curPos);
        SetNewStepT2(curPos.pos, curPos.pos, 1);
    }

    private IntPos SetNewStepT2(IntPos curPos, IntPos fromPos, int c = 0)
    {
        if (c > 1)
        {
            directions = directions.OrderBy(x => random.Next()).ToArray();
            c = 0;
        }
        c++;
        bool findSmt = false;
        for (int i = 0; i < 4; i++)
        {
            IntPos res = CheckCell(curPos, fromPos, directions[i].intPos, 1);
            //Debug.Log("CheckCell  " + directions[i] + " res " + res + "  :   " + (res != curPos) + " cur:pos " + curPos + "  from " + fromPos);
            if (res != curPos)
            {

                fromPos = curPos;
                curPos = res;

                if (random.Next(0, 100) < 13)
                {
                    var resObs = DoOstacle(curPos, fromPos, directions[i].side);
                    if (resObs != curPos)
                    {
                        fromPos = curPos;
                        curPos = resObs;
                        //grid[curPos.I, curPos.J].cell = CellType.free;
                        SetNewStepT2(curPos, fromPos, c);
                        break;
                    }
                }
                else
                {

                    grid[curPos.I, curPos.J].cell = CellType.free;
                    history.Add(grid[curPos.I, curPos.J]);
                    SetNewStepT2(curPos, fromPos, c);
                    findSmt = true;
                    break;
                }
            }
        }
        /*
        if (!findSmt)
        {
            //Debug.Log("branch is over " + history.Count);
            for (int i = 0; i < history.Count; i+=4)
            //for (int i = history.Count - 1; i > 0; --i)
            {
                for (int j = 0; j < 4; j++)
                {
                    IntPos res = CheckCell(history[i].pos, history[i].pos, directions[j].intPos, 1);
                    if (res != curPos)
                    {
                        if (brachLevel < 4 && childBranchNumber < 22)
                        {
                            MazeBranch mb = new MazeBranch(grid, random, size, brachLevel,mazeController);
                            childBranchNumber++;
                            mb.DoBranch(history[i]);
                        }
                        //SetNewStepT2(history[i].pos, history[i].pos, c);
                    }
                }
            }
        }
        */
        return curPos;
    }

    private IntPos DoOstacle(IntPos curPos, IntPos fromPos, Side side)
    {
        //int c = mazeController.Obstacles.Count;
        //int index = random.Next(0, c - 1);
        Obstacle o = new Obstacle();//mazeController.Obstacles[index];
        //o.LoadDefaut();
        o.Rotate(side);
        bool b = o.Check(curPos, grid);
        if (b)
        {
            Debug.Log("fromPos " + fromPos);
            return o.FillGrid(curPos, grid);
        }
        return curPos;
    }

    private IntPos CheckCell(IntPos curPos, IntPos fromPos, IntPos offset, int t2 = 0)
    {
        if (curPos.I < 0 || curPos.J < 0)
            return curPos;
        try
        {
            if (offset.I == 0)
            {
                if (curPos.J + offset.J < size - t2 && curPos.J + offset.J >= t2)
                {
                    if (fromPos.I == curPos.I && fromPos.J == curPos.J + offset.J)
                        return curPos;
                    if (grid[curPos.I, curPos.J + offset.J].cell == CellType.wall)
                    {
                        if (curPos.I - 1 >= 0 && curPos.I + 1 < size)
                        {
                            if (grid[curPos.I - 1, curPos.J + offset.J].cell == CellType.wall &&
                                grid[curPos.I + 1, curPos.J + offset.J].cell == CellType.wall)
                            {
                                return new IntPos(curPos.I, curPos.J + offset.J);
                            }
                        }
                    }
                }
                return curPos;
            }
            else if (offset.J == 0)
            {
                if (curPos.I + offset.I < size - t2 && curPos.I + offset.I >= t2)
                {
                    if (fromPos.I == curPos.I + offset.I && fromPos.J == curPos.J)
                    {
                        return curPos;
                    }
                    if (grid[curPos.I + offset.I, curPos.J].cell == CellType.wall)
                    {
                        if (curPos.J - 1 >= 0 && curPos.J + 1 < size)
                        {
                            if (grid[curPos.I + offset.I, curPos.J - 1].cell == CellType.wall &&
                                grid[curPos.I + offset.I, curPos.J + 1].cell == CellType.wall)
                            {
                                return new IntPos(curPos.I + offset.I, curPos.J);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("bug " + fromPos + "  " + curPos);
            throw ex;
        }
        
        return curPos;
    }
}

