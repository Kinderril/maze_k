using System;
using UnityEngine;

public class BallChecker : MonoBehaviour {

    // Use this for initialization
    private Ball ball;

    void OnTriggerEnter(Collider other)
    {
        var star = other.GetComponent<BlockElement>();
        if (star != null)
        {
            switch (star.type)
            {
                case CellType.star:
                    ball.GetStar(star);
                    Destroy(star);
                    star.transform.position = Vector3.up*-10000;
                    break;
                case CellType.jumer:
                    ball.AddForse(new Vector3(0,105,0));
                    break;
                case CellType.respawn:
                    ball.ToRespawn();
                    break;
            }

        }
    }

	void Start ()
	{
	    ball = GetComponentInParent<Ball>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
