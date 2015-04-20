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
    private Vector3 respawnPoint;
    public Vector3 andVelocity;
    public float emitRate = 10f;
    private float p_emitRate = 15f;
    private bool isPowerfull = false;
    private float endBombTime;
    public GameObject bombBall;
    public GameObject respawnObject;

    public bool IsPowerfull
    {
        get { return isPowerfull; }
        set
        {
            isPowerfull = value;
            bombBall.SetActive(isPowerfull);
        }
    }

    void Awake()
    {

    }
    private void Start()
    {
        //bombBall = GetComponentInChildren<MeshFilter>().gameObject;
        bombBall.SetActive(false);
        m_Rigidbody = GetComponent<Rigidbody>();
        emmiter = GetComponentInChildren<ParticleSystem>();
        // Set the maximum angular velocity.
        m_Rigidbody.maxAngularVelocity = m_MaxAngularVelocity;
    }

    public void Move(Vector2 moveDirection)
    {
        moveDirection *= power;

        if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength))
        {
            if (m_UseTorque)
            {
                m_Rigidbody.AddTorque(new Vector3(moveDirection.x, 0, moveDirection.y));
            }
            else
            {
                m_Rigidbody.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y),ForceMode.Acceleration);
            }

        }
        else
        {
            m_Rigidbody.AddForce(new Vector3(moveDirection.x/5, 0, moveDirection.y/5));
            
        }

        var d = Mathf.Clamp(m_Rigidbody.angularVelocity.magnitude/2 -3, 0, 9);
        if (emmiter != null)
            emmiter.emissionRate = d * p_emitRate;
    }

    void Update()
    {
        andVelocity = m_Rigidbody.angularVelocity;
        //CheckBomb();
    }

    public void Init(GameController gameController)
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        StopVelocity();
        this.gameController = gameController;
    }

    public void AddForse(Vector3 moveDirection)
    {
        m_Rigidbody.AddForce(moveDirection);
    }

    public void GetStar(BlockElement star)
    {
        gameController.GetStar();
        SetRespawnPoint(star);
    }

    public void SetRespawnPoint(BlockElement element)
    {
        respawnPoint = new Vector3(element.transform.position.x, element.transform.position.y + 4, element.transform.position.z);
        respawnObject.transform.position = respawnPoint;
        respawnObject.gameObject.SetActive(true);
    } 

    public void ToRespawn()
    {
        StopVelocity();
        transform.position = respawnPoint;
    }

    public void StopVelocity()
    {
        if (m_Rigidbody != null)
        {
            //m_Rigidbody.drag = Vector3.zero;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void StartPlay(Vector3 startPos,ControlType control)
    {
        transform.position = startPos;
        respawnPoint = startPos;
        if (control == ControlType.gyroscope)
        {
            p_emitRate = emitRate*1.4f;
        }
        else
        {
            p_emitRate = emitRate;
        }
    }

    public void StartGame()
    {
        power = Mathf.Abs(power);
        IsPowerfull = false;
        respawnObject.gameObject.SetActive(false);
        StopVelocity();
    }

    public void HitWall(BlockElement block)
    {
        if (isPowerfull)
        {
            Destroy(block.gameObject);
            gameController.maze.SetFree(block.transform);
            IsPowerfull = false;
        }
        /*
        if (m_Rigidbody.angularVelocity.sqrMagnitude > 17)
        {
            Handheld.Vibrate();
        }*/
        //WallElement w = (WallElement) block;
        //m_Rigidbody.AddForce(new Vector3(andVelocity.x, 0, andVelocity.z) * -0.5f, ForceMode.Impulse);
        //w.ChangeColor();
    }

    public void BombActivate()
    {
       // endBombTime = Time.time + 3f;
        IsPowerfull = true;
        //Debug.Log("BombActivate " + endBombTime);
    }
    /*
    private void CheckBomb()
    {
        if (isPowerfull)
        {
            if (endBombTime < Time.time)
            {
                Debug.Log("Bomb   DEActivate");
                IsPowerfull = false;
            }
        }
    }*/
}
