using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public struct IntPos
{
    public int I;
    public int J;

    public IntPos(int i, int j)
    {
        I = i;
        J = j;
    }
    
    public override string ToString()
    {
        return I + ":" + J;
    }

    public static bool operator ==(IntPos c1, IntPos c2)
    {
        return c1.I == c2.I && c1.J == c2.J;
    }

    public static bool operator !=(IntPos c1, IntPos c2)
    {
        return c1.I != c2.I || c1.J != c2.J;
    }

    public static IntPos operator -(IntPos x, IntPos y)
    {
        return new IntPos(x.I-y.I,x.J-y.J);
    }

    public static IntPos operator +(IntPos x, IntPos y)
    {
        return new IntPos(x.I + y.I, x.J + y.J);
    }
}

public class GridInfo
{
    private CellType cell;
    public IntPos pos;
    public int Id;
    public Side rotate;
    public ObstacleParameters obsParams;
    public bool isBuild = false;

    public GridInfo(CellType cellType,int i,int j)
    {
        Cell = cellType;
        pos = new IntPos(i,j);
    }

    

    public CellType Cell
    {
        get { return cell; }
        set { 
            cell = value;
            switch (cell)
            {
                case CellType.free:
                    Id = 4;
                    break;
                case CellType.wall:
                    Id = 1;
                    break;
                case CellType.star:
                    Id = 3;
                    break;
                case CellType.jumer:
                    Id = 2;
                    break;
                case CellType.teleport:
                    Id = 5;
                    break;
            }
        }
    }
}

public enum CellType
{
    wall,
    free,
    star,
    jumer,
    floor,
    respawn,
    obstacle,
    teleport,
}

public class MazeBuilder
{
    private int size = 20;
    private GridInfo[,] grid;
    //private IntPos curPosition;
    private IntPos[] directions = new IntPos[4] { new IntPos(0, 1), new IntPos(0, -1), new IntPos(1, 0), new IntPos(-1, 0) };
    private System.Random random;
    private int step = 0;
    private List<GridInfo> history = new List<GridInfo>();
    public int mazeDifficalty = 1;


    public MazeBuilder(Action<GridInfo[,], IntPos,int> onBuildComplete, int seed, int size, int maxStarCount,MazeController mcontroller)
    {
        this.size = size;
        random = new System.Random(seed);
        grid = new GridInfo[size, size];
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
                grid[i, j] = new GridInfo(CellType.wall,i,j);
        }
        IntPos startPos = new IntPos(1,1);
        /*
        for (int i = 0; i <6; i++)
        {
            startPos = new IntPos(random.Next(size / 4, size / 2), random.Next(size / 4, size / 2));
            SetNewStep(startPos, startPos);
            grid[startPos.I, startPos.J].cell = CellType.free;
            
        }*/
        MazeBranch branch = new MazeBranch(grid, random, size, 0, mcontroller);
        startPos = new IntPos(random.Next(size / 4, size / 2), random.Next(size / 4, size / 2));
        grid[startPos.I, startPos.J].Cell = CellType.free;
        branch.DoBranch(grid[startPos.I, startPos.J]);
        
        MazeBranch branch1 = new MazeBranch(grid, random, size, 0, mcontroller);
        var startPos2 = new IntPos(random.Next(size / 4, size / 2), random.Next(size / 4, size / 2));
        grid[startPos2.I, startPos2.J].Cell = CellType.free;
        branch1.DoBranch(grid[startPos2.I, startPos2.J]);
         
        /*
        grid[startPos.I, startPos.J].cell = CellType.free;
        history.Add(grid[startPos.I, startPos.J]);
        SetNewStepT2(startPos, startPos);
        */
        //setting BlockElement
        int freeCount = 0;
        int obstacleCount = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (grid[i, j].Cell == CellType.free)
                    freeCount++;
                if (grid[i, j].obsParams != null)
                {
                    obstacleCount++;
                }
            }
            
        }
        mazeDifficalty = (int)(freeCount/9f + obstacleCount*2.6f) + Mathf.Clamp(15 - seed,0,99);
        Debug.Log("seed:" + seed + "wallsCount: " + freeCount + " obstacleCount: " + obstacleCount + "  mazeDifficalty:" + mazeDifficalty);
        var stars = GetrandomList(maxStarCount-1,GetFreePos());
        if (stars.Count != maxStarCount)
        {
            Debug.LogError("GFR45 " + stars.Count  + "   " + (maxStarCount));
        }
        foreach (var a in stars)
            a.Cell = CellType.star;

        /*
        var exits = GetrandomList(2-1, GetFreePos());
        foreach (var a in exits)
          6
         * a.cell = CellType.end;
        */

        var teleports = GetrandomList(1, GetFreePos());
        Debug.Log("teleports " + teleports.Count);
        foreach (var a in teleports)
        {
            a.Cell = CellType.teleport;
        }

        
        Debug.Log("MazeBuilder start from " + startPos);
        //Debug.Log("finsish " + step);
        onBuildComplete(grid, startPos, mazeDifficalty);
    }

    public List<T> GetrandomList<T>(int count, List<T> list, bool evenly = true)
    {
        if (list.Count < count)
            return list;
        var outher = new List<T>();
        if (evenly)
        {
            int step = list.Count/(count+1);
            for (int i = 0; i <= count; i++)
            {
                int index = step * i + random.Next(step/3, step*2/3);
                var rndelement = list[index];
                outher.Add(rndelement);
            }
        }
        else
        {
            var indexes = new List<int>();
            while (outher.Count <= count)
            {
                int index = random.Next(0, list.Count);
                var rndelement = list[index];
                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                    outher.Add(rndelement);
                }
            }
        }
        return outher;
    }

    public List<GridInfo> GetFreePos()
    {
        List<GridInfo> list = new List<GridInfo>();
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                if (grid[i, j].Cell == CellType.free)
                    list.Add(grid[i, j]);
            }
        }
        return list;
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
            IntPos res = CheckCell(curPos, fromPos, directions[i],1);
            //Debug.Log("CheckCell  " + directions[i] + " res " + res + "  :   " + (res != curPos) + " cur:pos " + curPos + "  from " + fromPos);
            if (res != curPos)
            {
                fromPos = curPos;
                curPos = res;
                grid[curPos.I, curPos.J].Cell = CellType.free;
                history.Add(grid[curPos.I, curPos.J]);
                SetNewStepT2(curPos, fromPos, c);
                findSmt = true;
                break;
            }
        }
        if (!findSmt)
        {
            Debug.Log("else " + history.Count);
            for (int i = history.Count-1; i >0; --i)
            {
                IntPos res = CheckCell(history[i].pos, history[i].pos, directions[i], 1);
                if (res != curPos)
                {
                    SetNewStepT2(history[i].pos, history[i].pos, c);
                }
            }
            
        }
        return curPos;
    }

    private IntPos SetNewStep(IntPos curPos, IntPos fromPos, int c = 0)
    {
        step++;
        if (c > 1)
        {
            directions = directions.OrderBy(x => random.Next()).ToArray();
            c = 0;
        }
        c++;
        for (int i = 0; i < 4; i++)
        {
            IntPos res = CheckCell(curPos, fromPos, directions[i]);
            //Debug.Log("CheckCell  " + directions[i] + " res " + res + "  :   " + (res != curPos) + " cur:pos " + curPos + "  from " + fromPos);
            if (res != curPos)
            {
                //Debug.Log(i + "||||   "+ directions[i] + " res " + res);
                fromPos = curPos;
                curPos = res;
                grid[curPos.I, curPos.J].Cell = CellType.free;
                SetNewStep(curPos, fromPos, c);

                //if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
                if (random.Next(0, 100) < 13)
                {
                    SetNewStep(curPos, fromPos, c);
                }

                break;
            }
        }


        return curPos;
    }
    
    private IntPos CheckCell(IntPos curPos, IntPos fromPos,IntPos offset,int t2 = 0)
    {
        if (offset.I == 0)
        {
            if (curPos.J + offset.J < size-t2 && curPos.J + offset.J >= t2)
            {
                if (fromPos.I == curPos.I && fromPos.J == curPos.J + offset.J)
                    return curPos;
                if (grid[curPos.I, curPos.J + offset.J].Cell == CellType.wall)
                {
                    if (curPos.I - 1 >= 0 && curPos.I + 1 < size)
                    {
                        if (grid[curPos.I - 1, curPos.J + offset.J].Cell == CellType.wall &&
                            grid[curPos.I + 1, curPos.J + offset.J].Cell == CellType.wall)
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
            if (curPos.I + offset.I < size-t2 && curPos.I + offset.I >= t2)
            {
                if (fromPos.I == curPos.I + offset.I && fromPos.J == curPos.J)
                {
                    return curPos;
                }
                if (grid[curPos.I + offset.I, curPos.J].Cell == CellType.wall)
                {
                    if (curPos.J - 1 >= 0 && curPos.J + 1 < size)
                    {
                        if (grid[curPos.I + offset.I, curPos.J - 1].Cell == CellType.wall &&
                            grid[curPos.I + offset.I, curPos.J + 1].Cell == CellType.wall)
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
