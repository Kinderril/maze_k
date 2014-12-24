using UnityEngine;
using UnityEngine.EventSystems;

public class UIControls : MonoBehaviour , IPointerDownHandler , IPointerUpHandler , IDragHandler {

	// Use this for initialization
    private Ball ball;
    private Vector2 basePosition;
    private Vector2 curPower;
    public float distancePercent = 0.5f;
    private float d;
    //public Image StartDragImage;
    private Vector2 _v;
    private Vector2 _r;
    public float maxAcc  = 1;

	void Start ()
	{
	    basePosition = new Vector2(Screen.width/2, Screen.height/2);
        //distancePercent
        d = Mathf.Min(Screen.width / 2, Screen.height / 2) * distancePercent;
        //Debug.Log(basePosition);
	}

    public void Init(Ball ball)
    {
        this.ball = ball;
    }
	
	// Update is called once per frame
	void Update () {
        if (ball != null)
	        ball.Move(curPower);
	}

    public void OnPointerDown(PointerEventData eventData)
    {
       // basePosition = eventData.position;
        //StartDragImage.gameObject.SetActive(false);
        //StartDragImage.transform.position = basePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        curPower = Vector2.zero;
        //StartDragImage.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {

        _v = (eventData.position - basePosition);
        if (_v.magnitude > d)
        {
            _r = _v - _v.normalized*distancePercent;
            _r = _r/200;
            curPower = new Vector2(Mathf.Clamp(_r.x, -maxAcc, maxAcc), Mathf.Clamp(_r.y, -maxAcc, maxAcc));
        }
        else
        {
            curPower = Vector2.zero;
        }
    }
}
