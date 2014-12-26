using System;
using System.Collections.Generic;
using System.Linq;
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

public class GridInfo
{
    public CellType cell;
    public IntPos pos;

    public GridInfo(CellType cellType,int i,int j)
    {
        cell = cellType;
        pos = new IntPos(i,j);
    }
}

public enum CellType
{
    wall,
    free,
    star,
    end,
    jumer,
    floor,
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

    public MazeBuilder(Action<GridInfo[,], IntPos> onBuildComplete, int seed, int size, int maxStarCount)
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
        MazeBranch branch = new MazeBranch(grid,random,size,0);
        startPos = new IntPos(random.Next(size / 4, size / 2), random.Next(size / 4, size / 2));
        grid[startPos.I, startPos.J].cell = CellType.free;
        branch.DoBranch(grid[startPos.I, startPos.J]);
        /*
        grid[startPos.I, startPos.J].cell = CellType.free;
        history.Add(grid[startPos.I, startPos.J]);
        SetNewStepT2(startPos, startPos);
        */
        //setting BlockElement
        var stars = GetrandomList(maxStarCount-1,GetFreePos());
        foreach (var a in stars)
            a.cell = CellType.star;

        var exits = GetrandomList(2-1, GetFreePos());
        foreach (var a in exits)
            a.cell = CellType.end;

        var jumps = GetrandomList(6, GetFreePos());
        foreach (var a in jumps)
            a.cell = CellType.jumer;


        Debug.Log("MazeBuilder start from " + startPos);
        Debug.Log("finsish " + step);
        onBuildComplete(grid, startPos);
    }

    public List<T> GetrandomList<T>(int count, List<T> list)
    {
        if (list.Count < count)
            return list;
        List<T> outher = new List<T>();
        int index = 0;
        while (outher.Count <= count)
        {
            index = random.Next(0, list.Count);
            var rndelement = list[index];
            if (!outher.Contains(rndelement));
                outher.Add(rndelement);
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
                if (grid[i, j].cell == CellType.free)
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
                grid[curPos.I, curPos.J].cell = CellType.free;
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
                grid[curPos.I, curPos.J].cell = CellType.free;
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
            if (curPos.I + offset.I < size-t2 && curPos.I + offset.I >= t2)
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
