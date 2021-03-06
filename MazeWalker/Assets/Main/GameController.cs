﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;

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
    private ControlType ctype = ControlType.gyroscope;
    public List<BaseWindow> allWindows = new List<BaseWindow>();
    private GameStage gameStage = GameStage.mainMenu;
    private float startTime;
    private float endTime;
    private ResultController resultController;
    public FollowTarget mainCameraHolder;
    public FaceBookController faceBook;
    public SettingsBeh settings;
    public int Sec2Level = 60;
   // private ResultLevel _lastLevelResultLevel;

    private GameStage GameStage
    {
        //get { return gameStage; }
        set
        {
            var curWindow = allWindows.FirstOrDefault(x => x.gameStage == value);
            WindowManager.WindowOn(curWindow);
            gameStage = value;
        }
    }

    public ResultController ResultController
    {
        get { return resultController; }
    }

    void Awake()
    {
        bControls = maze.GetComponent<UIControls>();
        gControls = maze.GetComponent<GControls>();
        sControls = maze.GetComponent<SwipeControls>();
        settings.gameController = this;
        resultController =new ResultController();
        ball.Init(this);
        WindowManager.Init(allWindows,this);
        WindowManager.WindowOn(startWindow);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        faceBook = new FaceBookController();
        Advertisement.Initialize("47546");
    }

    public void StartGame(int p)
    {
        startTime = Time.time;
        curStars = 0;
        Debug.Log("StartGame " + ctype);
        bControls.enabled = sControls.enabled = gControls.enabled = false;
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
        size = resultController.GetMazeSize(p);
        maze.BuildMaze(size, maxStars, p);

        GameStage = GameStage.game;
    }

    IEnumerator EndByTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndGame();
    }

    public void EndGame(bool withExitWindow = true)
    {
        main_UI.UnPause();
        ball.gameObject.SetActive(false);
        if (withExitWindow)
        {
            endTime = Time.time;
            resultController.AddResult(endTime - startTime, maze.seed, ctype, curStars);
            GameStage = GameStage.end;
        }
        else
        {
            Time.timeScale = 1;
            GameStage = GameStage.mainMenu;
        }
        //StopAllCoroutines();
    }


    private void onComplete(Vector3 startPos,int sizeStatus)
    {

        ball.StartPlay(startPos, ctype);
        StartCoroutine(WaitStart(sizeStatus));
    }

    IEnumerator WaitStart(int sizeStatus)
    {
        yield return new WaitForSeconds(3);
        ball.gameObject.SetActive(true);
        ball.StartGame();
        main_UI.InitUI(maze.seed);
        Sec2Level = sizeStatus;
    } 

   
    
    void Update()
    {
        if (gameStage == GameStage.game && Time.time - startTime > 0)
        {
            if (Time.time - startTime > Sec2Level)
            {
                EndGame();
            }
        }
    }

    public void PlusTime(int pl)
    {
        endTime += pl;
        Sec2Level += pl;
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
        return (Sec2Level - (Time.time - startTime)).ToString("00.0");
    }


    public List<Result> LoadResults()
    {
        return resultController.Load();
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

    public void CleatResults()
    {
        resultController.Clear();
    }

    public void SettingsOn()
    {
        GameStage = GameStage.setting;
    }

    public void OnMainMenu()
    {
        GameStage = GameStage.mainMenu;
    }
}

