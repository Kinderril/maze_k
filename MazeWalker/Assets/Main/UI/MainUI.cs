using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Text spendLabel;
    public Button buttonExit;

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
            star2.transform.localPosition = new Vector3(47 * i - 100, -4.6f, 0);
            allStars.Add(star2.GetComponent<StarContainer>());
        }
        foreach (var starContainer in allStars)
        {
            starContainer.Close();
        }
        for (int i = 0; i < gc.curStars; i++)
        {
            allStars[i].Open();
        } 
        UpdateSpendLabel();

    }

    private void UpdateSpendLabel()
    {
        spendLabel.text = GameController.ResultController.PointsToSpend + "";
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
        //GameController.StartGame();
    }

    public void OnPauseClick()
    {
        Debug.Log("OnPauseClick");
        GameController.Pause();
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

    public void OnBombClicked()
    {
        if (CheckStars(7))
        {
            var pos = GameController.ball.transform.position;
            
            var list =
                GameController.maze.currentWalls.Where(
                    x => (x.transform.position - pos).sqrMagnitude < 2f).ToArray();
           // Debug.Log("list " + list.Length + "    " + GameController.maze.walls.Count);
            for (int i = 0; i < list.Length; i++)
            {
                var block = list[i];
                Destroy(block.gameObject);
                GameController.maze.SetFree(block.transform);
                GameController.maze.currentWalls.Remove(block);
                
                //i--;
                //IsPowerfull = false;
            }
            //GameController.ball.BombActivate();
        }
    }

    public void OnClickZoom(Button btn)
    {
        if (CheckStars(5))
        {
            Time.timeScale = 1;
            buttonExit.gameObject.SetActive(false);
            StartCoroutine(zooming());
        }
    }

    public void OnRespawnClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(GameController.ball.transform.position, -Vector3.up, out hit))
        {
            var wall = hit.collider.gameObject.GetComponent<BlockElement>();
            if (wall != null)
            {
                if (wall.type == CellType.free)
                {
                    if (CheckStars(2))
                    {
                        GameController.ball.SetRespawnPoint(wall);
                    }
                }
            }
        }
    }

    private bool CheckStars(int count )
    {
        if (GameController.ResultController.PointsToSpend < count)
        {
            return false;
        }
        GameController.ResultController.PointsToSpend -= count;
        UpdateSpendLabel();
        return true;
    }

    public void OnTimeClick()
    {
        if (CheckStars(9))
        {
            GameController.PlusTime(10);
        }
    }

    IEnumerator zooming()
    {
        Vector3 a = GameController.mainCameraHolder.transform.position;
        float yy = a.y;
        //GameController.mainCameraHolder.transform.position = new Vector3(a.x, 50, a.z);
        GameController.mainCameraHolder.StartFly();
        yield return new WaitForSeconds(2.2f);
        buttonExit.gameObject.SetActive(true);
        GameController.mainCameraHolder.EndFly();
        GameController.mainCameraHolder.transform.position = new Vector3(a.x, yy, a.z);
    
    }  
}
