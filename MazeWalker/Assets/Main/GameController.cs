
using UnityEngine;
using UnitySampleAssets.Vehicles.Ball;

public class GameController : MonoBehaviour
{
    public MainUI mainUI;
    public MazeController maze;
    public Ball ball;
    public int maxStars;
    public int curStars;
    public int size;

    void Awake()
    {
        mainUI.InitUI(maxStars, this);
    }

    public void StartGame()
    {
        maze.Init(onComplete);
        maze.BuildMaze(size,maxStars);
    }

    private void onComplete(Vector3 startPos)
    {
        ball.transform.position = startPos;
    }

}

