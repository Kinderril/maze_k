using System;
using UnityEngine;

public class ObstacleParameters 
{
    public int id = 0;
    public int w;
    public int h;
    public IntPos enter;
    public IntPos exit;
    public Side rotation;
    public bool withStar = false;

    public ObstacleParameters(int id,int w, int h, IntPos enter, IntPos exit, Side rotation, bool withStar)
    {
        this.id = id;
        this.w = w;
        this.h = h;
        this.enter = enter;
        this.exit = exit;
        this.rotation = rotation;
        this.withStar = withStar;
    }

    public void Rotate(Side totateTo)
    {
        if (totateTo == Side.up)
            return;

        if (rotation == Side.up)
        {
            switch (totateTo)
            {
                case Side.down:
                    enter.I = -exit.I;
                    enter.J = -exit.J;

                    exit.I = -exit.I;
                    exit.J = -exit.J;
                    w = -w;
                    h = -h;
                    break;
                case Side.left:
                    IntPos en2 = enter;
                    IntPos ex2 = exit;

                    exit.I = -ex2.J;
                    exit.J = ex2.I;

                    enter.I = -en2.J;
                    enter.J = en2.I;
                    int h1 = h;
                    h = w;
                    w = -h1;

                    break;
                case Side.right:
                    IntPos en1 = enter;
                    IntPos ex1 = exit;

                    exit.I = ex1.J;
                    exit.J = -ex1.I;

                    enter.I = en1.J;
                    enter.J = -en1.I;

                    int h2 = h;
                    h = -w;
                    w = h2;
                    break;
            }
        }
        else
        {
            Debug.LogError("Can rotate only from up " + rotation);
        }
        rotation = totateTo;
    }

    public bool Check(IntPos pos, GridInfo[,] grid)
    {
        IntPos GSP = pos - enter;
        IntPos GEP = new IntPos(GSP.I + w, GSP.J + h);
        int sI = GSP.I;
        int sJ = GSP.J;
        int eI = GEP.I;
        int eJ = GEP.J;
        var signI = Math.Sign(eI - sI);
        var signJ = Math.Sign(eJ - sJ);
        int gl = grid.GetLength(0);

        if (signI == 0 || signJ == 0)
        {
            Debug.Log("bad bad abdad");
            return false;
        }

        for (int i = sI; signI == -1 ? i > eI : i < eI; i += signI)
        {
            for (int j = sJ; signJ == -1 ? j > eJ : j < eJ; j += signJ)
            {
                if (i < 0 || j < 0 || i >= gl || j >= gl)
                    return false;

                if (grid[i, j].Cell != CellType.wall)
                    return false;
            }
        }
        return true;
    }


    public IntPos FillGrid(IntPos pos, GridInfo[,] grid)
    {
        IntPos GSP = pos - enter;
        IntPos GEP = new IntPos(GSP.I + w, GSP.J + h);
       // Debug.Log("FillGrid start ");
       // Debug.Log("GSP" + (GSP) + "  GEP:" + GEP + "  rot:" + rotation + "  exit:" + exit + "  enter:" + enter);
        int sI = GSP.I;
        int sJ = GSP.J;
        int eI = GEP.I;
        int eJ = GEP.J;
        var signI = Math.Sign(eI - sI);
        var signJ = Math.Sign(eJ - sJ);
        int i = 0;
        int j = 0;
        for (i = sI; signI == -1 ? i > eI : i < eI; i += signI)
        {
            for (j = sJ; signJ == -1 ? j > eJ : j < eJ; j += signJ)
            {
                grid[i, j].Cell = CellType.obstacle;
                grid[i, j].Id = 99;
                //Debug.Log("check..." + i + "  " + j);
                if (i == pos.I && j == pos.J)
                {
                    //Debug.Log("find core " + id);
                    grid[i, j].obsParams = this;
                    grid[i, j].Id = id;
                    grid[i, j].rotate = rotation;
                }

            }
        }
      //  Debug.Log("FillGrid end " + (GSP + exit));
        return GSP + exit;
    }
}

