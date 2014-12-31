using System;
using UnityEngine;

public class Obstacle //: MonoBehaviour
{
    public int w;
    public int h;
    private IntPos enter;
    private IntPos exit;
    public Side rotation;
    public bool withStar = false;
    private ObstacleParameters parameters;


    public Vector2 Enter;
    public Vector2 Exit;

    public Obstacle()
    {
        w = 3;
        h = 3;
        enter = new IntPos(1, 0);
        exit = new IntPos(1, 2);
        rotation = Side.up;
    }

    public void Init()
    {
    /*
        rotation = Side.up;
        enter = new IntPos((int)Enter.x, (int)Enter.y);
        exit = new IntPos((int)Exit.x, (int)Exit.y);
        parameters = new ObstacleParameters(w, h, enter, exit, rotation, withStar);
        Debug.Log("parameters " + parameters);
     */ 
    }

    public void LoadDefaut()
    {
        w = parameters.w;
        h = parameters.h;
        enter = parameters.enter;
        exit = parameters.exit;
        rotation = parameters.rotation;
        withStar = parameters.withStar;
        Debug.Log("load default " + rotation);
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

    public override string ToString()
    {
        return rotation + " w:" + w + " h:" + h + " enter:" + enter + " exit:"+exit;
    }

    public bool Check(IntPos pos,GridInfo[,] grid)
    {
        IntPos GSP = pos - enter;
        IntPos GEP = new IntPos(GSP.I + w, GSP.J + h);
        int gl = grid.GetLength(0);
        /*
        int minI = Mathf.Min(GSP.I, GEP.I);
        int minJ = Mathf.Min(GSP.J, GEP.J);

        int maxI = Mathf.Max(GSP.I, GEP.I);
        int maxJ = Mathf.Max(GSP.J, GEP.J);
        */
        int sI = GSP.I;
        int sJ = GSP.J;

        int eI = GEP.I;
        int eJ = GEP.J;

        var signI = Math.Sign(eI - sI);
        var signJ = Math.Sign(eJ - sJ);

        if (signI == 0 || signJ == 0)
        {
            Debug.Log("bad bad abdad");
            return false;
        }

        for (int i = sI; signI == -1 ? i > eI : i < eI ; i += signI)
        {
            for (int j = sJ; signJ == -1 ? j > eJ : j < eJ; j += signJ)
            {
                if (i < 0 || j < 0 || i >= gl || j >= gl)
                    return false;

                if (grid[i, j].cell != CellType.wall)
                    return false;
            }
        }
        return true;
    }


    public IntPos FillGrid(IntPos pos, GridInfo[,] grid)
    {
        IntPos GSP = pos - enter;
        IntPos GEP = new IntPos(GSP.I + w, GSP.J + h);
        /*int minI = Mathf.Min(GSP.I, GEP.I);
        int minJ = Mathf.Min(GSP.J, GEP.J);

        int maxI = Mathf.Max(GSP.I, GEP.I);
        int maxJ = Mathf.Max(GSP.J, GEP.J);
        */
        Debug.Log("FillGrid start ");
        Debug.Log("GSP" + (GSP) + "  GEP:" + GEP + "  rot:" + rotation + "  exit:" + exit);
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
                grid[i, j].cell = CellType.obstacle;
                Debug.Log("check..." + i + "  " + j);
                if (i == pos.I && j == pos.J)
                {
                    //Debug.Log("find core " + grid[i, j].pos);
                    grid[i, j].Id = 999;
                }
                /*
                if (i == pos.I && j == pos.J)
                {
                    //Debug.Log("find core " + grid[i, j].pos);
                    grid[i, j].Id = 999;
                }*/

            }
        }
        Debug.Log("FillGrid end ");
        return grid[i, j].pos;
    }
}

