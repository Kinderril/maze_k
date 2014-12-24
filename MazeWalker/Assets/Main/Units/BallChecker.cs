using System;
using UnityEngine;

public class BallChecker : MonoBehaviour {

    // Use this for initialization
    private Ball ball;

    void OnTriggerEnter(Collider other)
    {
        var star = other.GetComponent<blockElement>();
        if (star != null)
        {
            switch (star.type)
            {
                case BlockType.star:
                    Destroy(star.gameObject);
                    ball.gameController.GetStar();
                    break;
                case BlockType.end:
                    Destroy(star.gameObject);
                    ball.gameController.EndGame();
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
