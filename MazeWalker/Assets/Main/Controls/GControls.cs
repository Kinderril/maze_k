using UnityEngine;
using UnityEngine.UI;

public class GControls : MonoBehaviour
{

    private Ball ball;
    public float power = 8f;

	void Update ()
	{
        Vector3 a = Input.acceleration * power;
	    Vector2 b = new Vector2(a.x, a.y);
        ball.Move(b);
	}

    internal void Init(Ball ball)
    {
        this.ball = ball;
    }
}
