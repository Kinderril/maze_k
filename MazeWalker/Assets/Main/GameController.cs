using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MainUI main_UI;
    public WindowStart startWindow;
    public MazeController maze;
    public Ball ball;
    public int maxStars;
    public int curStars;
    public int size;
    private UIControls bControls;
    private GControls gControls;
    private SwipeControls sControls;
    private ControlType ctype = ControlType.border;
    public List<BaseWindow> allWindows = new List<BaseWindow>();
    private GameStage gameStage = GameStage.mainMenu;
    private float startTime;
    private float endTime;
    private Result lastLevelResult;

    private GameStage GameStage
    {
        get { return gameStage; }
        set
        {
            var curWindow = allWindows.FirstOrDefault(x => x.gameStage == value);
            WindowManager.WindowOn(curWindow);
            gameStage = value;
        }
    }

    void Awake()
    {
        bControls = maze.GetComponent<UIControls>();
        gControls = maze.GetComponent<GControls>();
        sControls = maze.GetComponent<SwipeControls>();
        ball.Init(this);
        WindowManager.Init(allWindows,this);
        WindowManager.WindowOn(startWindow);
    }

    public void StartGame(int p)
    {
        startTime = Time.time;
        curStars = 0;
        GameStage = GameStage.game;
        ball.gameObject.SetActive(true);
        Debug.Log("StartGame " + ctype);
        bControls.enabled = sControls.enabled = false;
        switch (ctype)
        {
            case ControlType.border:
                bControls.enabled = true;
                bControls.Init(ball);
                break;
            case ControlType.gyroscope:
                gControls.enabled = true;
                gControls.Init(ball);
                break;
            case ControlType.swipe:
                sControls.enabled = true;
                sControls.Init(ball);
                break;
        }
        maze.Init(onComplete);
        maze.BuildMaze(size, maxStars,p);
    }

    public void EndGame(bool withExitWindow = true)
    {
        ball.gameObject.SetActive(false);
        if (withExitWindow)
        {
            endTime = Time.time;
            lastLevelResult = new Result(endTime - startTime, maze.seed, ctype);
            GameStage = GameStage.end;
        }
        else
        {
            Time.timeScale = 1;
            GameStage = GameStage.mainMenu;
        }
    }


    private void onComplete(Vector3 startPos)
    {
        ball.StartPlay(startPos);
        main_UI.InitUI(maze.seed);
    }

    public void GetStar()
    {
        curStars++;
        main_UI.SetStar(curStars);
        if (curStars == maxStars)
        {
            EndGame(true);
        }
    }

    public void OnEndConfirm()
    {
        GameStage = GameStage.mainMenu;
    }

    public string GetCurSpendTime()
    {
        return (Time.time - startTime).ToString("##.##");
    }

    public Result GetLastResult()
    {
        return lastLevelResult;
    }

    public void SaveResult()
    {
        lastLevelResult.Save();
    }

    public List<Result> LoadResults()
    {
        List<Result> list = new List<Result>();
        if (PlayerPrefs.HasKey(Result.RESULT_SAVE))
        {
            string s= PlayerPrefs.GetString(Result.RESULT_SAVE);
            //Debug.Log("RESULT_SAVE " + s);
            list.AddRange(from item in s.Split(Result.DELEMITER_HIGH) where item.Length > 5 select new Result(item));
        }
        return list;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameStage = GameStage.pause;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        GameStage = GameStage.game;
    }

    public void ChangeControlType(ControlType controlType)
    {
        ctype = controlType;
    }

}

