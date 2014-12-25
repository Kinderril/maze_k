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
                    Destroy(star.gameObject);
                    ball.gameController.GetStar();
                    break;
                case CellType.end:
                    Destroy(star.gameObject);
                    ball.gameController.EndGame();
                    break;
                case CellType.jumer:
                    ball.AddForse(new Vector3(0,105,0));
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
