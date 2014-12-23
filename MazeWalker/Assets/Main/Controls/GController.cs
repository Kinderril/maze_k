using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GController : MonoBehaviour
{

    public Text label;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
            label.text = ""+ Input.acceleration;
        /*
        if (Input.gyro.enabled)
        else
        {
            label.text = "Not working";
            Input.gyro.enabled = true;
        }*/
	}
}
