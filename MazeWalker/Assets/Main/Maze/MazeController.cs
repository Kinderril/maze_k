using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeController : MonoBehaviour {

	// Use this for initialization
    private MazeBuilder mazeGrid;
    public GameObject block;
    public GameObject Star;
    public GameObject EndElement;
    public int seed;
    private int size;
    private int stars;
    private Action<Vector3> onComplete;

	void Start ()
	{

	}

    public void Init(Action<Vector3> onComplete)
    {
        this.onComplete = onComplete;
        if (block == null || Star == null)
            Debug.LogError("BLOCK OR STAR IS NULL!!!");
    }

    public void BuildMaze(int size,int stars)
    {
        Clear();
        this.size = size;
        this.stars = stars;
        seed = Random.Range(0, 99999);
        mazeGrid = new MazeBuilder(OnBuildComplete, seed, size, stars);
    }

    private void OnBuildComplete(GridInfo[,] obj,IntPos startPos)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                switch (obj[i,j].cell)
                {
                    case CellType.wall:
                        GameObject go = Instantiate(block, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        go.transform.parent = transform;
                        break;
                    case CellType.end:
                        GameObject e = Instantiate(EndElement.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        e.transform.parent = transform;
                        break;
                    case CellType.free:
                        break;
                    case CellType.star:
                        GameObject s = Instantiate(Star.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        s.transform.parent = transform;
                        break;
                }
            }
            
        }
        onComplete(new Vector3(startPos.I, 2, startPos.J));
        //SBall.transform.position = new Vector3(startPos.I ,2,startPos.J);
    }

    public void Clear()
    {
        var cc = GetComponentsInChildren<BoxCollider>();
        Debug.Log("cc " + cc.Length);
        var tt = cc.Where(x => x.name.Contains(block.gameObject.name) || x.name.Contains(EndElement.gameObject.name) || x.name.Contains(Star.gameObject.name));
        Debug.Log("cc " + tt.Count());
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
