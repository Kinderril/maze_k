using UnityEngine;
using UnityEngine.UI;

public class GControls : MonoBehaviour
{

    private Ball ball;

   // public Text label;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector3 a = Input.acceleration*10;
	    Vector3 b = new Vector3(a.x, 0, a.y);
        ball.AddForse(b);
	    //label.text = b.ToString();

	    /*
        if (Input.gyro.enabled)
        else
        {
            label.text = "Not working";
            Input.gyro.enabled = true;
        }*/
	}

    internal void Init(Ball ball)
    {
        this.ball = ball;
    }
}
