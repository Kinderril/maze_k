using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    public Text startlabel;
    private int maxStars;
    private GameController gameController;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnStartClick(Button btn)
    {
        gameController.StartGame();
        btn.gameObject.SetActive(false);
    }

    public void InitUI(int maxStars, GameController gameController)
    {
        this.gameController = gameController;
        this.maxStars = maxStars;
    }

    public void SetStar(int count)
    {
        startlabel.text = count+"/" + maxStars;
    }
}
