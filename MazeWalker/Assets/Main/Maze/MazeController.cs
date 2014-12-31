using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeController : MonoBehaviour {

	// Use this for initialization
    private MazeBuilder mazeGrid;
    //public BlockElement block;
    //public BlockElement Star;
    //public BlockElement EndElement;
    //public BlockElement Jumper;
    public Transform WallContainer;
    public List<BlockElement> blocks = new List<BlockElement>(); 
    public int seed;
    private int size;

    public List<Obstacle> Obstacles = new List<Obstacle>();

    //private int stars;
    private Action<Vector3> onComplete;

	void Start ()
	{
	    foreach (var obstacle in Obstacles)
	    {
	        obstacle.Init();
	    }
     //   Debug.Log(TestObstacle);
     //   TestObstacle.Rotate(Side.right);
     //   Debug.Log(TestObstacle);
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
       // var f = blocks.FirstOrDefault(x => x.type == CellType.free);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
				Debug.Log("WTF??");
                var b = blocks.FirstOrDefault(x => x.type == obj[i, j].cell);
                if (b != null)
                {
                    Quaternion q = Quaternion.identity;
                    Vector3 v = new Vector3(i, 0, j);
                   /* if (b.type != CellType.wall)
                    {
                        GameObject go1 = Instantiate(f.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        go1.transform.parent = transform;
                    }*/
                    Transform parent = transform;
                    if (b.type == CellType.wall)
                    {
                        parent = WallContainer;
                    }
                    else if (b.type == CellType.obstacle)
                    {
                        if (obj[i, j].Id > 1)
                        {
                            v = new Vector3(i,1,j);
                        }
                    }
                    GameObject go = Instantiate(b.gameObject, v, q) as GameObject;
                    go.transform.parent = parent;
                    BlockElement block = go.GetComponent<BlockElement>();
                    block.I = i;
                    block.J = j;
                }
            }
            
        }
        onComplete(new Vector3(startPos.I, 2, startPos.J));
        //SBall.transform.position = new Vector3(startPos.I ,2,startPos.J);
    }

    public void Clear()
    {
        var cc = GetComponentsInChildren<BoxCollider>();
      //  Debug.Log("cc " + cc.Length);
        var tt = cc.Where(x => !x.name.Contains("Plane") && !x.name.Contains("Maze"));
      //  Debug.Log("cc " + tt.Count());
        foreach (var componentsInChild in tt)
        {
            componentsInChild.transform.position = Vector3.down*-100000;
            Destroy(componentsInChild.gameObject);
        }
        //BuildMaze(size, stars);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
