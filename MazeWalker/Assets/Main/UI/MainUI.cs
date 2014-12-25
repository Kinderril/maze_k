using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    public Text startlabel;
    private int maxStars;
    private GameController gameController;
   // public GameObject gameCamera;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnStartClick(Button btn)
    {
        Debug.Log("OnStartClick");
        gameController.StartGame();
//        gameCamera.gameObject.SetActive(true);
       // btn.gameObject.SetActive(false);
    }

    public void OnRestartClick()
    {
        Debug.Log("OnRestartClick");
        
    }

    public void InitUI(int maxStars, GameController gameController)
    {
        this.gameController = gameController;
        this.maxStars = maxStars;
        SetStar(0);
    }

    public void SetMaxStar(int count)
    {
        maxStars = count;
        startlabel.text = count + "/" + maxStars;
    }

    public void SetStar(int count)
    {
        startlabel.text = count+"/" + maxStars;
    }
}
