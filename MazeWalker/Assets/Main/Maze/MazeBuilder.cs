using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Random = UnityEngine.Random;
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
}

public enum CellType
{
    wall,
    free,
    star,
}

public class MazeBuilder
{
    private int size = 20;
    private CellType[,] grid;
    //private IntPos curPosition;
    private IntPos[] directions = new IntPos[4] { new IntPos(0, 1), new IntPos(0, -1), new IntPos(1, 0), new IntPos(-1, 0) };
    private System.Random random;
    private int step = 0;

    public MazeBuilder(Action<CellType[,], IntPos> onBuildComplete, int seed ,int size,int maxStarCount)
    {
        this.size = size;
        random = new System.Random(seed);
        grid = new CellType[size, size];
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
                grid[i,j] = CellType.wall;
        }
        IntPos startPos = new IntPos(1,1);
        for (int i = 0; i <6; i++)
        {
            startPos = new IntPos(random.Next(size / 4, size / 2), random.Next(size / 4, size / 2));
            SetNewStep(startPos, startPos);
            grid[startPos.I, startPos.J] = CellType.free;
            
        }
        //setting star
        int curStars = 0;
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                if (grid[i, j] == CellType.free && curStars <= maxStarCount)
                {
                    if (Random.Range(0f, 1f) < 0.3f)
                    {
                        grid[i, j] = CellType.star;
                    }
                }
                if (curStars > maxStarCount)
                    break;
            }
        }
        Debug.Log("MazeBuilder start from " + startPos);
        
        Debug.Log("finsish " + step);
        
        onBuildComplete(grid, startPos);
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
                grid[curPos.I, curPos.J] = CellType.free;
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
    
    private IntPos CheckCell(IntPos curPos, IntPos fromPos,IntPos offset)
    {
        if (offset.I == 0)
        {
            if (curPos.J + offset.J < size && curPos.J + offset.J >= 0)
            {
                if (fromPos.I == curPos.I && fromPos.J == curPos.J + offset.J)
                    return curPos;
                if (grid[curPos.I, curPos.J + offset.J] == CellType.wall)
                {
                    if (curPos.I - 1 >= 0 && curPos.I + 1 < size)
                    {
                        if (grid[curPos.I - 1, curPos.J + offset.J] == CellType.wall &&
                            grid[curPos.I + 1, curPos.J + offset.J] == CellType.wall)
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
            if (curPos.I + offset.I < size && curPos.I + offset.I >= 0)
            {
                if (fromPos.I == curPos.I + offset.I && fromPos.J == curPos.J)
                {
                    return curPos;
                }
                if (grid[curPos.I + offset.I, curPos.J] == CellType.wall)
                {
                    if (curPos.J - 1 >= 0 && curPos.J + 1 < size)
                    {
                        if (grid[curPos.I + offset.I, curPos.J - 1] == CellType.wall &&
                            grid[curPos.I + offset.I, curPos.J + 1] == CellType.wall)
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
