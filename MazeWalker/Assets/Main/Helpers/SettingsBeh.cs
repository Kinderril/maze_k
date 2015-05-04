
using System.Collections.Generic;
using UnityEngine;
public class SettingsBeh : MonoBehaviour
{
    private const string BALL_INDEX = "BALL_INDEX";
    private const string TRAIL_INDEX = "TRAIL_INDEX";

    public GameController gameController;
    public List<Material> ballMaterials = new List<Material>();
    public List<GameObject> trails = new List<GameObject>();
    public int pointsToOpen = 10;
    public int pointsToOpenTrails = 20;
    private int ballIndex = 0;
    private int trailIndex = 0;

    public int TrailIndex
    {
        get { return trailIndex; }
        set
        {
            trailIndex = value;
            PlayerPrefs.SetInt(TRAIL_INDEX, trailIndex);
            int i = 0;
            foreach(var item in trails)
            {
                item.gameObject.SetActive(i == trailIndex);
                i++;
                Debug.Log(item + "  " + (i == trailIndex));
            }
        }
    }

    public int BallIndex
    {
        get { return ballIndex; }
        set {
            ballIndex = value;
            PlayerPrefs.SetInt(BALL_INDEX, ballIndex);
        }
    }

    void Awake()
    {
        ballIndex = PlayerPrefs.GetInt(BALL_INDEX, 0);
        TrailIndex = PlayerPrefs.GetInt(TRAIL_INDEX, 0);
    }

}

