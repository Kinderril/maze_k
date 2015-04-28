
using UnityEngine;

public class FreeElement : BlockElement
{

    public Material OffMaterial;
    public Material OnMaterial;
    public bool isOn = false;
    private float time;
    public float materialLenght = 3;
    protected Renderer renderer;
    private Material[] OnList;
    private Material[] OffList;


    void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        OnList =  new Material[2]
        {
            renderer.materials[0], OnMaterial
        };
        OffList = new Material[2]
        {
            renderer.materials[0], OffMaterial
        };
    }

    public void DoAction(bool isOn)
    {
        this.isOn = isOn;
       // Debug.Log("do action " + isOn + "     " + renderer.materials[1]);
        if (isOn)
        {
            renderer.materials = OnList;
            time = 0;
        }
        else
        {
            renderer.materials = OffList;
        }
    }

    void Update()
    {
        if (isOn)
        {
            time += Time.deltaTime;
            renderer.materials[1].SetFloat("_TimeM", time/materialLenght);
            if (time > materialLenght)
            {
                DoAction(false);
            }
        }
    }
}

