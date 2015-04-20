using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderChecker : MonoBehaviour 
{
    public Material m;
    float time = 0;
    /*void Update()
    {
        m.SetVector("_Pos", transform.position);
    }*/
	// Use this for initialization
    public List<Material> materials = new List<Material>();
    void OnTriggerEnter(Collider other)
    {
        CheckAndDo(other,true);
    }
    
    /*
    void OnTriggerExit(Collider other)
    {
        CheckAndDo(other,false);
    }
    */
    private void CheckAndDo(Collider other, bool add)
    {
        
        var block = other.GetComponent<BlockElement>();
        if (block != null && block.type == CellType.free)
        {
            if (add){
                materials.Add(block.material);
            }else{
                materials.Remove(block.material);
            }
        }
    }

    void Update(){
        time += Time.deltaTime * 10;
        if (time > Mathf.PI*2){
            time = 0;
        }

        foreach(var a in materials){
            if (a != null)
            {
                a.SetVector("_Pos", transform.position);
                //a.SetFloat("_TimeM", time);
            }
        }
    }
    
}
