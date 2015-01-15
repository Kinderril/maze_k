
using UnityEngine;

public class WallElement : BlockElement
{
    public int w;
    private WallContainer wc;
    public bool isBuilded = false;

    void Start()
    {
        wc = transform.parent.GetComponent<WallContainer>();
    }

    public void ChangeColor()
    {
        int index = Random.Range(0, wc.wallMaterials.Count);
        GetComponent<Renderer>().material = wc.wallMaterials[index];
    }

    internal void SetBuild(int p1, int p2)
    {
        isBuilded = true;
        I = p1;
        J = p2;
    }
}

