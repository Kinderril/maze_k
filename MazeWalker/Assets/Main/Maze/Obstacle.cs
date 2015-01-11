using System;
using UnityEngine;

[RequireComponent(typeof(BlockElement))]
public class Obstacle : MonoBehaviour
{
    //public int id;
    public int w;
    public int h;
    private IntPos enter;
    private IntPos exit;
    public Side rotation;
    public bool withStar = false;
    private ObstacleParameters parameters;

    public Vector2 Enter;
    public Vector2 Exit;
    private BlockElement blockElement;

    void Awake()
    {
        blockElement = GetComponent<BlockElement>();
    }
    
    public void Init()
    {
        blockElement = GetComponent<BlockElement>();
        rotation = Side.up;
        enter = new IntPos((int)Enter.x, (int)Enter.y);
        exit = new IntPos((int)Exit.x, (int)Exit.y);
        //parameters = new ObstacleParameters(id,w, h, enter, exit, rotation, withStar);
        //Debug.Log("parameters " + parameters);
     
    }

    public ObstacleParameters GetParams()
    {
        return new ObstacleParameters(blockElement.Id, w, h, enter, exit, rotation, withStar);
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

    public override string ToString()
    {
        return rotation + " w:" + w + " h:" + h + " enter:" + enter + " exit:"+exit;
    }
}

