using UnityEngine;
using System.Collections;

public class ControlChanger : MonoBehaviour {

	// Use this for initialization
    private Ball b;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        var bc = other.GetComponent<BallChecker>();
        if (bc == null)
        {
           // Debug.Log("W111T???"); 
            return;
        }
        b = bc.Ball;
        b.power *= -1;
       // Debug.Log("Ball IN^^^^^");
    }
    void OnTriggerExit(Collider other)
    {
        var bc = other.GetComponent<BallChecker>();
        if (bc == null)
        {
           // Debug.Log("W222T???"); 
            return;
        }
        b.power *= -1;
       // Debug.Log("Ball OUT>>>>>");
    }
}
