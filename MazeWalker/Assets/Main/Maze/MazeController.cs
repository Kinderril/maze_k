using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeController : MonoBehaviour {

    private MazeBuilder mazeGrid;
    public Transform ObstacleContainer;
    public Transform StarContainer;
    public Transform FreeContainer;
    public Transform WallContainer;
    public Transform OtherContainer;
    public List<BlockElement> blocks = new List<BlockElement>();
    public int seed;
    private int size;
    private List<Obstacle> obstacles;
    private Action<Vector3> onComplete;

    private Dictionary<CellType, List<BlockElement>> currentBlocks;


    public List<Obstacle> Obstacles
    {
        get { return obstacles; }
    }
    public Dictionary<CellType, List<BlockElement>> CurrentBlocks
    {
        get { return currentBlocks; }
    }

    //private int stars;

	void Start ()
	{
        obstacles = new List<Obstacle>();
        foreach (var block in blocks)
        {
            Obstacle obs = block.GetComponent<Obstacle>();
            if (obs != null)
            {
                block.type = CellType.obstacle;
                obstacles.Add(obs);
            }
        }

	    foreach (var obstacle in obstacles)
	    {
	        obstacle.Init();
	    }

        Debug.Log("Obstacle count = " + obstacles.Count);
	}

    public void Init(Action<Vector3> onComplete)
    {
        this.onComplete = onComplete;
     //   if (block == null || Star == null)
      //      Debug.LogError("BLOCK OR STAR IS NULL!!!");
    }

    public void BuildMaze(int size,int stars,int isRandom)
    {
        Clear();
        this.size = size;
       // this.stars = stars;
        seed = isRandom <= 0 ? Random.Range(1, 99999) : isRandom;
        mazeGrid = new MazeBuilder(OnBuildComplete, seed, size, stars,this);
    }

    private void OnBuildComplete(GridInfo[,] obj,IntPos startPos)
    {
        currentBlocks = new Dictionary<CellType, List<BlockElement>>();
        var f = blocks.FirstOrDefault(x => x.type == CellType.free);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GridInfo gi = obj[i, j];
                var b = blocks.FirstOrDefault(x => x.Id == gi.Id);
                if (b != null)
                {
                    Quaternion q = Quaternion.identity;
                    Vector3 v = new Vector3(i, 0, j);
                    if (b.type == CellType.teleport)
                    {
                        Debug.Log("!!!!!!!!!!");
                    }

                    if (b.type != CellType.wall && b.type != CellType.obstacle) 
                    {
                        GameObject go1 = Instantiate(f.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        go1.transform.parent = FreeContainer;
                    }
                    Transform parent = transform;
                    //Debug.Log("!!!!!!!   " + i + "   " + j + "   " + b.type + "  " +obj[i, j].Id);
                    switch (b.type)
                    {
                        case CellType.wall:
                            parent = WallContainer;
                            break;
                        case CellType.obstacle:
                            parent = ObstacleContainer;
                            break;
                        case CellType.star:
                            parent = StarContainer;
                            break;
                        case CellType.free:
                            parent = FreeContainer;
                            break;
                        default:
                            parent = OtherContainer;
                            break;
                    }
                    if (b.type == CellType.obstacle)
                    {
                        if (gi.obsParams != null)
                        {
                            int w_obst =  gi.obsParams.w ;
                            int h_obst = gi.obsParams.h ;
                            float cw = (w_obst - 1 * (Math.Sign(w_obst))) / 2f;
                            float ch = (h_obst - 1 * (Math.Sign(h_obst))) / 2f;
                            //Debug.Log("Obstacle setting   " + cw + "   " + ch + "  rotate:" + gi.rotate + " enter:" + gi.obsParams.enter + "  w_obst:" + w_obst + "  h_obst:" + h_obst + "  id:" + gi.obsParams.id);
                            q = Quaternion.Euler(GameUtils.GetV3BySide(gi.rotate));
                            //v = new Vector3(i , 0, j );
                            v = new Vector3(i + (cw - gi.obsParams.enter.I), 0, j + (ch - gi.obsParams.enter.J));
                            //Debug.Log("vvvv " + v);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    GameObject go = Instantiate(b.gameObject, v, q) as GameObject;
                    go.transform.parent = parent;

                    BlockElement block = go.GetComponent<BlockElement>();
                    //block.I = i;
                    //block.J = j;
                    block.Init(i,j);
                    if (!currentBlocks.ContainsKey(block.type))
                        currentBlocks.Add(block.type,new List<BlockElement>());
                    currentBlocks[block.type].Add(block);
                }
                else
                {
                   // if (obj[i, j].Id > 10)
                      //  Debug.Log("can't find id " + obj[i, j].Id);
                }
            }
            
        }
        onComplete(new Vector3(startPos.I, 2, startPos.J));
        //SBall.transform.position = new Vector3(startPos.I ,2,startPos.J);
    }

    public void Clear()
    {

        //var cc = GetComponentsInChildren<BoxCollider>();
      //  Debug.Log("cc " + cc.Length);
      //  var tt = cc.Where(x => !x.name.Contains("Plane") && !x.name.Contains("Maze"));
      //  Debug.Log("cc " + tt.Count());
        ClearContainer(ObstacleContainer);
        ClearContainer(StarContainer);
        ClearContainer(WallContainer);
        ClearContainer(FreeContainer);
        ClearContainer(OtherContainer);
        
            /*
        foreach (var componentsInChild in tt)
        {
            Destroy(componentsInChild.gameObject);
        }*/
        //BuildMaze(size, stars);
    }

    private void ClearContainer(Transform tr)
    {

        foreach (Transform child in tr.transform)
            Destroy(child.gameObject);
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
