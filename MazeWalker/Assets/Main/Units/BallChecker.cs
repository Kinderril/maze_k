using System;
using UnityEngine;

public class BallChecker : MonoBehaviour {

    // Use this for initialization
    private Ball ball;
    private bool teleported = false;

    public Ball Ball
    {
      get { return ball; }
    }
    void OnTriggerEnter(Collider other)
    {
        var block = other.GetComponent<BlockElement>();
        if (block != null)
        {
            switch (block.type)
            {
                case CellType.star:
                    ball.GetStar(block);
                   // Destroy(block);
                    block.transform.position = Vector3.up*-10000;
                    break;
                case CellType.jumer:
                    ball.AddForse(new Vector3(0,105,0));
                    break;
                case CellType.respawn:
                    ball.ToRespawn();
                    break;
                case CellType.teleport:
                    if (teleported)
                    {
                        teleported = false;
                        return;
                    }
                   // Debug.Log("inc: " + block);
                    var l = Ball.gameController.maze.CurrentBlocks[CellType.teleport];
                    if (l.Count > 1)
                    {
                        BlockElement exitTeleport = block;
                        while (exitTeleport == block)
                        {
                            int index = UnityEngine.Random.Range(0, l.Count);
                            exitTeleport = l[index];
                        }
                       // Debug.Log("out: "+exitTeleport);
                        teleported = true;
                        ball.transform.position = exitTeleport.transform.position;
                    }
                    else
                    {
                        Debug.LogWarning("smt wrong only one teleport");
                    }
                    break;
            }
            block.Play();

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
