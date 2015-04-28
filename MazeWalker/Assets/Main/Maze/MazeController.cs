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
    public List<WallElement> walls = new List<WallElement>();
    public int seed;
    private int size;
    private List<Obstacle> obstacles;
    private Action<Vector3,int> onComplete;
    public GameObject connector;
    private BlockElement freeBlock;
    public ParticleSystem wallDestroyEffect;
    public List<GameObject> currentWalls; 
    private bool noWalls = false;

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
	    blocks = blocks.Where(x => x != null).ToList();
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

//        Debug.Log("Obstacle count = " + obstacles.Count);
	}

    public void Init(Action<Vector3,int> onComplete)
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
        noWalls = Random.Range(0f, 100f) <= 3f;
        mazeGrid = new MazeBuilder(OnBuildComplete, seed, size, stars,this);
    }

    private void OnBuildComplete(GridInfo[,] grid_maze,IntPos startPos,int difficalty)
    {
        currentWalls = new List<GameObject>();
        currentBlocks = new Dictionary<CellType, List<BlockElement>>();
        freeBlock = blocks.FirstOrDefault(x => x.type == CellType.free);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GridInfo gi = grid_maze[i, j];
                Quaternion q = Quaternion.identity;
                Vector3 v = new Vector3(i, 0, j);

                var b = blocks.FirstOrDefault(x => x.Id == gi.Id);
                if (b != null)
                {
                    if (b.type != CellType.wall && b.type != CellType.obstacle && b.type != CellType.free) 
                    {
                        var go1 = Instantiate(freeBlock.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        go1.transform.parent = FreeContainer;
                    }
                    Transform parent = transform;
                    //Debug.Log("!!!!!!!   " + i + "   " + j + "   " + b.type + "  " +grid_maze[i, j].Id);
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
                    GameObject go;
                    if (b.type != CellType.wall)
                    {
                        go = Instantiate(b.gameObject, v, q) as GameObject;
                        go.transform.parent = parent;
                    }
                    else
                    {
                        return;
                       // go = SetFree(gi,v,q);
                       // go.transform.parent = parent;
                    }

                    BlockElement block = go.GetComponent<BlockElement>();
                    block.Init(i,j);
                    if (!currentBlocks.ContainsKey(block.type))
                        currentBlocks.Add(block.type,new List<BlockElement>());
                    currentBlocks[block.type].Add(block);
                }
                else
                {
                    if (gi.Id == 1)
                    {

                        GameObject go;
                        go = SetWall(gi,grid_maze, v, q);
                        if (go != null)
                        {
                            gi.isBuild = true;
                            go.transform.parent = WallContainer;
                            currentWalls.Add(go);
                        }
                    }
                }
            }
            
        }
       // SeconProcess(grid_maze);
        if (noWalls)
            difficalty = 999;
        onComplete(new Vector3(startPos.I, 2, startPos.J), difficalty);
        //SBall.transform.position = new Vector3(startPos.I ,2,startPos.J);
    }

    private void SeconProcess(GridInfo[,] grid_maze)
    {
        float yy = 1.6f;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GridInfo gi = grid_maze[i, j];
                if (gi.Id == 1 && gi.isBuild && gi.pos.I < size - 1 && gi.pos.J < size - 1)
                {
                    if (grid_maze[gi.pos.I + 1, gi.pos.J].isBuild)
                    {
                        SetConnector(new Vector3(gi.pos.I + 0.5f,yy, gi.pos.J), true);
                    }
                    else if (grid_maze[gi.pos.I, gi.pos.J + 1].isBuild)
                    {
                        SetConnector(new Vector3(gi.pos.I,yy ,gi.pos.J + 0.5f),false);
                    }
                }
            }
        }
    }

    public bool IsBonusLevel()
    {
        return noWalls;
    }

    private void SetConnector(Vector3 p, bool side)
    {
        Quaternion q = Quaternion.Euler(GameUtils.GetV3BySide(side?Side.up : Side.left));
        Instantiate(connector, p, q);
        //Debug.Log("Set connector " + vector2 + "   p"+p);
    }

    public void SetFree(Transform transform)
    {
        wallDestroyEffect.transform.position = transform.position + new Vector3(0,1.7f,0);
        wallDestroyEffect.enableEmission = true;
        wallDestroyEffect.Play();
        var go = Instantiate(freeBlock.gameObject, transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = FreeContainer;
    }


    private GameObject SetWall(GridInfo gi,GridInfo[,] obj,Vector3 v,Quaternion q)
    {
        if (noWalls)
        {
            return null;
        }
        int index = 0;
        if (gi.pos.I > 0 && gi.pos.J > 0 && gi.pos.I < size-1 && gi.pos.J < size-1)
        {
            int boolCount = 0;
            bool b1 = obj[gi.pos.I + 1, gi.pos.J].Cell == CellType.wall;
            bool b2 = obj[gi.pos.I, gi.pos.J + 1].Cell == CellType.wall;
            bool b3 = obj[gi.pos.I - 1, gi.pos.J].Cell == CellType.wall;
            bool b4 = obj[gi.pos.I, gi.pos.J - 1].Cell == CellType.wall;
            if (b1)
                boolCount++;
            if (b2)
                boolCount++;
            if (b3)
                boolCount++;
            if (b4)
                boolCount++;
            if (boolCount ==4)
            {
                return null;
            }
            if (boolCount == 0)
            {
                index = 1;
            }

        }


        //GameObject go  = ;
        //go.GetComponent<WallElement>().SetBuild(gi.pos.I, gi.pos.J);
        return Instantiate(walls[index].gameObject, v, q) as GameObject;
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
