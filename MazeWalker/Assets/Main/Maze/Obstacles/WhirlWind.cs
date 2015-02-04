using UnityEngine;
using System.Collections;

public class WhirlWind : MonoBehaviour {

    Ball ball;
    float distSqr;
    Vector3 dir;
    Vector3 forse;
    public float power = 100;
    void OnTriggerEnter(Collider other)
    {
        var bc = other.GetComponent<BallChecker>();
        if (bc == null)
        {
            return;
        }
        ball = bc.BallOwner;
    }
    void OnTriggerExit(Collider other)
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (ball != null)
        {
            dir = ball.transform.position - transform.position;
            distSqr = (dir).sqrMagnitude;
            forse = -dir.normalized * (2f - distSqr);
            ball.AddForse(forse * power);
        }
    }
}
