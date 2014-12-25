
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeBranch
{
    private GridInfo[,] grid;
    private IntPos[] directions = new IntPos[4] { new IntPos(0, 1), new IntPos(0, -1), new IntPos(1, 0), new IntPos(-1, 0) };
    private System.Random random;
    private int size;
    private List<GridInfo> history = new List<GridInfo>();
    private int brachLevel;
    private int childBranchNumber = 0;

    public MazeBranch(GridInfo[,] grid, System.Random random, int size,int prevBranchLevel)
    {
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
            IntPos res = CheckCell(curPos, fromPos, directions[i], 1);
            //Debug.Log("CheckCell  " + directions[i] + " res " + res + "  :   " + (res != curPos) + " cur:pos " + curPos + "  from " + fromPos);
            if (res != curPos)
            {
                fromPos = curPos;
                curPos = res;
                grid[curPos.I, curPos.J].cell = CellType.free;
                history.Add(grid[curPos.I, curPos.J]);
                SetNewStepT2(curPos, fromPos, c);
                findSmt = true;
                break;
            }
        }
        if (!findSmt)
        {
            //Debug.Log("branch is over " + history.Count);
            for (int i = 0; i < history.Count; i+=4)
            //for (int i = history.Count - 1; i > 0; --i)
            {
                for (int j = 0; j < 4; j++)
                {
                    IntPos res = CheckCell(history[i].pos, history[i].pos, directions[j], 1);
                    if (res != curPos)
                    {
                        if (brachLevel < 4 && childBranchNumber < 22)
                        {
                            MazeBranch mb = new MazeBranch(grid, random, size, brachLevel);
                            childBranchNumber++;
                            mb.DoBranch(history[i]);
                        }
                        //SetNewStepT2(history[i].pos, history[i].pos, c);
                    }
                }
            }

        }
        return curPos;
    }

    private IntPos CheckCell(IntPos curPos, IntPos fromPos, IntPos offset, int t2 = 0)
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
        return curPos;
    }
}

