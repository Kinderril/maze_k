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
    private UIControls uiControls;
    private GControls gControls;
    private ControlType ctype = ControlType.ui;
    public List<BaseWindow> allWindows = new List<BaseWindow>();
    private GameStage gameStage = GameStage.mainMenu;
    private float startTime;
    private float endTime;

    public GameStage GameStage
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
        uiControls = maze.GetComponent<UIControls>();
        gControls = maze.GetComponent<GControls>();
        ball.Init(this);
        WindowManager.Init(allWindows,this);
        WindowManager.WindowOn(startWindow);
    }

    public void StartGame()
    {
        startTime = Time.time;
        curStars = 0;
        //main_UI.SetMaxStar(maxStars);
        //main_UI.SetStar(curStars);
        GameStage = GameStage.game;
        main_UI.InitUI(maze.seed);
        ball.gameObject.SetActive(true);
        switch (ctype)
        {
            case ControlType.ui:
                uiControls.Init(ball);
                break;
            case ControlType.g:
                break;
        }
        maze.Init(onComplete);
        maze.BuildMaze(size,maxStars);
    }

    public void EndGame()
    {
        endTime = Time.time;
        GameStage = GameStage.end;
        ball.gameObject.SetActive(false);
    }

    private void onComplete(Vector3 startPos)
    {
        ball.transform.position = startPos;
    }

    public void GetStar()
    {
        curStars++;
        main_UI.SetStar(curStars);
    }

    public void OnEndConfirm()
    {
        GameStage = GameStage.mainMenu;
    }

    public string GetCurSpendTime()
    {
        return (Time.time - startTime).ToString("##.##");
    }

    public string GetTime()
    {
        return (endTime - startTime).ToString("##.##");
    }
}

