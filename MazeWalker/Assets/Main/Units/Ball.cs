using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private bool m_UseTorque = true; // Whether or not to use torque to move the ball.
    [SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
    [SerializeField] public float power = 1;

    private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
    private Rigidbody m_Rigidbody;
    public GameController gameController;
    private ParticleSystem emmiter;


    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        emmiter = GetComponentInChildren<ParticleSystem>();
        // Set the maximum angular velocity.
        GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
    }
    public void Move(Vector2 moveDirection)
    {
        moveDirection *= power;

        if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength))
        {
            if (m_UseTorque)
            {
                // ... add torque around the axis defined by the move direction.
                m_Rigidbody.AddTorque(new Vector3(moveDirection.x, 0, moveDirection.y));
            }
            else
            {
                // Otherwise add force in the move direction.
                m_Rigidbody.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y));
            }

        }
        // If using torque to rotate the ball...
        /*if (m_UseTorque)
        {
            // ... add torque around the axis defined by the move direction.
            //m_Rigidbody.AddTorque(new Vector3(moveDirection.x, 0, moveDirection.y));
        }
        else
        {
            // Otherwise add force in the move direction.
        }
        */
        var d = Mathf.Clamp(m_Rigidbody.angularVelocity.magnitude/2 -3, 0, 9);
        if (emmiter != null)
            emmiter.emissionRate = d*11;
    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void AddForse(Vector3 moveDirection)
    {
        m_Rigidbody.AddForce(moveDirection);
    }
}
