using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnitySampleAssets.Vehicles.Ball;
using Random = UnityEngine.Random;

public class MazeController : MonoBehaviour {

	// Use this for initialization
    private MazeBuilder mazeGrid;
    public GameObject block;
    public Star star;
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
        if (block == null || star == null)
            Debug.LogError("BLOCK OR STAR IS NULL!!!");
    }

    public void BuildMaze(int size,int stars)
    {
        this.size = size;
        this.stars = stars;
        seed = Random.Range(0, 99999);
        mazeGrid = new MazeBuilder(OnBuildComplete, seed, size, stars);
    }

    private void OnBuildComplete(CellType[,] obj,IntPos startPos)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                switch (obj[i,j])
                {
                    case CellType.wall:
                        GameObject go = Instantiate(block, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        go.transform.parent = transform;
                        break;
                    case CellType.free:
                        break;
                    case CellType.star:
                        GameObject s = Instantiate(star.gameObject, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        s.transform.parent = transform;
                        break;
                }
            }
            
        }
        onComplete(new Vector3(startPos.I, 2, startPos.J));
        //SBall.transform.position = new Vector3(startPos.I ,2,startPos.J);
    }

    public void ReBuild()
    {
        var cc = GetComponentsInChildren<BoxCollider>();
        Debug.Log("cc " + cc.Length);
        foreach (var componentsInChild in cc.Where(x=>x.name.Contains("Cube")))
        {
            componentsInChild.transform.position = Vector3.down*-100000;
            Destroy(componentsInChild.gameObject);
        }
        BuildMaze(size, stars);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
