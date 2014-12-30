using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
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

    public void Init()
    {
        rotation = Side.up;
        enter = new IntPos((int)Enter.x, (int)Enter.y);
        exit = new IntPos((int)Exit.x, (int)Exit.y);
        parameters = new ObstacleParameters(w, h, enter, exit, rotation, withStar);
        Debug.Log("parameters " + parameters);
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

    public void Rotate(Side side)
    {
        if (side == Side.up)
            return;

        if (rotation == Side.up)
        {
            if (side != Side.down)
            {
                int w1 = w;
                w = h;
                h = w1;
            }
            switch (side)
            {
                case Side.down:
                    IntPos e1 = enter;
                    enter.J = exit.J;
                    enter.I = exit.I;
                    exit.J = e1.J;
                    exit.I = e1.I;
                    break;
                case Side.left:
                    IntPos en2 = enter;
                    IntPos ex2 = exit;

                    exit.I = h - en2.J - 1;
                    exit.J = en2.I;

                    enter.I = h - ex2.J - 1;
                    enter.J = ex2.I;
                    break;
                case Side.right:
                    IntPos en1 = enter;
                    IntPos ex1 = exit;

                    enter.I = en1.J;
                    enter.J = w- en1.I - 1;

                    exit.I = ex1.J;
                    exit.J = w - ex1.I - 1;

                    break;
            }
        }
        else
        {
            Debug.LogError("Can rotate only from up " + rotation);
        }
        rotation = side;
    }

    public override string ToString()
    {
        return rotation + " w:" + w + " h:" + h + " enter:" + enter + " exit:"+exit;
    }

    public bool Check(IntPos pos,GridInfo[,] grid)
    {
        Debug.Log("check start " + pos);
        int sI = pos.I - enter.I;
        int sJ = pos.J - enter.J;
        for (int i = sI; i < sI + w; i++)
        {
            for (int j = sJ; j < sJ + h; j++)
            {
                if (i < 0 || j < 0 || i>grid.Length || j>grid.Length)
                    return false;

                if (grid[i, j].cell != CellType.wall)
                    return false;

            }
        }
        return true;
    }


    public IntPos DoGrid(IntPos GSP, GridInfo[,] grid)
    {


        IntPos GEP = new IntPos(GSP.I + exit.I, GSP.J + exit.J);

        int sI = GSP.I - enter.I;
        int sJ = GSP.J - enter.J;

        
       // IntPos starPos = new IntPos(sI + enter.I, sJ + enter.J);
       // IntPos endPos = new IntPos(sI + exit.I, sJ + exit.J);

        Debug.Log("DoGrid start " + GSP + "  " + sI + "  " + sJ + "   " + rotation + " core " + enter + "   GSP: " + GSP + "  GEP:" + GEP);// + (sI - enter == enter) + );
        for (int i = sI; i < sI + w; i++)
        {
            for (int j = sJ; j < sJ + h; j++)
            {
                grid[i, j].cell = CellType.obstacle;
                Debug.Log("check..."  + i + "  " + j);
                if (i == GSP.I && j == GSP.J)
                {
                    Debug.Log("find core " + grid[i, j].pos);
                    grid[i, j].Id = 999;
                }
            }
        }
        
        Debug.Log("DoGrid end" + (exit + GSP) + "   " + rotation + "  exit:"+exit);
        return GEP;
    }
}

