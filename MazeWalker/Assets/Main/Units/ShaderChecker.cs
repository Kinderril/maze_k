using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderChecker : MonoBehaviour 
{
    public Material m;
    float time = 0;

    //public List<FreeElement> elements = new List<FreeElement>();


    void OnTriggerEnter(Collider other)
    {
        CheckAndDo(other,true);
    }
    
    
    void OnTriggerExit(Collider other)
    {
        //CheckAndDo(other,false);
    }
    
    private void CheckAndDo(Collider other, bool add)
    {

        var block = other.GetComponent<FreeElement>();
        if (block != null && block.type == CellType.free)
        {
            block.DoAction(add);
            /*
            if (add){
                elements.Add(block);
            }else{
                elements.Remove(block);
            }*/
        }
    }

    
}
