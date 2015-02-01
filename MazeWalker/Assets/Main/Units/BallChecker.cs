using System;
using UnityEngine;

public class BallChecker : MonoBehaviour {

    // Use this for initialization
    private Ball _ballOwner;
    private bool teleported = false;

    public Ball BallOwner
    {
      get { return _ballOwner; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
       
    }

    void OnTriggerEnter(Collider other)
    {
        var block = other.GetComponent<BlockElement>();
        if (block != null)
        {
            switch (block.type)
            {
                case CellType.star:
                    _ballOwner.GetStar(block);
                   // Destroy(block);
                    block.transform.position = Vector3.up*-10000;
                    break;
                case CellType.jumer:
                    _ballOwner.AddForse(new Vector3(0,105,0));
                    break;
                case CellType.respawn:
                    _ballOwner.ToRespawn();
                    break;
                case CellType.teleport:
                    if (teleported)
                    {
                        teleported = false;
                        return;
                    }
                   // Debug.Log("inc: " + block);
                    var l = BallOwner.gameController.maze.CurrentBlocks[CellType.teleport];
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
                        _ballOwner.StopVelocity();
                        _ballOwner.transform.position = new Vector3(exitTeleport.transform.position.x, exitTeleport.transform.position.y + 2, exitTeleport.transform.position.z); ;
                    }
                    else
                    {
                        Debug.LogWarning("smt wrong only one teleport");
                    }
                    break;
                case CellType.wall:
                    //Debug.Log("myNormal " + other.ra);
                    BallOwner.HitWall(block);
                    break;
            }
            block.Play();

        }
    }

	void Start ()
	{
	    _ballOwner = GetComponentInParent<Ball>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
