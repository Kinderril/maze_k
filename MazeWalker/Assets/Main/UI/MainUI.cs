using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseWindow
{

    //public Text starlabel;
    //private int maxStars;
    //private GameController gameController;
    public GameObject starsParent;
    public StarContainer star = null;
    List<StarContainer> allStars = new List<StarContainer>();

    public Text levelIdLabel;
    public Text TimeLabel;

	// Update is called once per frame
	void Update ()
	{
	    TimeLabel.text = GameController.GetCurSpendTime();
	}

    public override void Init(GameController gc)
    {
        base.Init(gc);
        for (int i = 0; i < gc.maxStars; i++)
        {
            GameObject star2 = Instantiate(star.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            star2.transform.parent = starsParent.transform;
            star2.transform.localPosition = new Vector3(32 + 47*i, -26, 0);
            allStars.Add(star2.GetComponent<StarContainer>());
        }
        foreach (var starContainer in allStars)
        {
            starContainer.Close();
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        foreach (var a in allStars)
        {
            Destroy(a.gameObject);
        }
        allStars.Clear();
    }

    public void OnStartClick(Button btn)
    {
        GameController.StartGame();
    }

    public void OnPauseClick()
    {
        Debug.Log("OnPauseClick");

    }

    public void InitUI(int levelId)
    {
        levelIdLabel.text = levelId.ToString();
        //this.gameController = gameController;
        //this.maxStars = maxStars;
        //SetStar(0);
    }

    public void SetMaxStar(int count)
    {
        //maxStars = count;
        //starlabel.text = count + "/" + maxStars;
    }

    public void SetStar(int count)
    {

        //starlabel.text = count+"/" + maxStars;
        foreach (var starContainer in allStars)
        {
            if(starContainer.Open())
                break;
        }
    }
}
