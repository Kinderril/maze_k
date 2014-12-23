using UnityEngine;
using System.Collections;

public class MazeMeshBuilder : MonoBehaviour {

	// Use this for initialization
    private MeshFilter m_filter;
    private Mesh mesh;
    public float w = 1f;
    public float h = 1f;

	void Awake ()
	{
	    m_filter = GetComponent<MeshFilter>();
	}

    void Start()
    {
        Init();
    }

    public void Init()
    {
        mesh = new Mesh();
        m_filter.mesh = mesh;

        //vertex
        Vector3[] v = new Vector3[3]
        {
            new Vector3(0,0),new Vector3(w,0),new Vector3(0,h)  
        };
        
        //trianles
        int[] trl = new int[3];
        trl[0] = 0;
        trl[1] = 1;
        trl[2] = 2;
        
        //normals
        Vector3[] normals = new Vector3[3];
        normals[0] = Vector3.forward;
        normals[1] = Vector3.forward;
        normals[2] = Vector3.forward;

        //uv
        Vector2[] uv = new Vector2[3];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);

        //exit
        mesh.vertices = v;
        mesh.triangles = trl;
        mesh.normals = normals;
        mesh.uv = uv;
        Debug.Log("mesh done");

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
