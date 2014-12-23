using UnityEngine;
using System.Collections;

public class BallChecker : MonoBehaviour {

    // Use this for initialization

    void OnTriggerEnter(Collider other)
    {
        var star = other.GetComponent<Star>();
        if (star != null)
            Debug.Log("Start!! " + other);
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
