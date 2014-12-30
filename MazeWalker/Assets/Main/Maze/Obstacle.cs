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
                    exit.J = e1.J;
                    break;
                case Side.left:
                    IntPos en2 = enter;
                    IntPos ex2 = exit;

                    exit.I = w - en2.J;
                    exit.J = en2.I;

                    enter.I = w - ex2.J;
                    enter.J = ex2.I;
                    break;
                case Side.right:
                    IntPos en1 = enter;
                    IntPos ex1 = exit;

                    enter.I = en1.J;
                    enter.J = h - en1.I;

                    exit.I = ex1.J;
                    exit.J = h - ex1.I;

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
                if (grid[i, j].cell != CellType.wall)
                    return false;

            }
        }
        return true;
    }

    public IntPos DoGrid(IntPos pos, GridInfo[,] grid)
    {
        int sI = pos.I - enter.I;
        int sJ = pos.J - enter.J;
        for (int i = sI; i < sI + w; i++)
        {
            for (int j = sJ; j < sJ + h; j++)
            {
                grid[i, j].cell = CellType.obstacle;
            }
        }
        return exit + pos;
    }
}

