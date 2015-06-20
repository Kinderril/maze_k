
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class WindowEnd : BaseWindow
{
    public Text cureentResult;
    public Text TimeText;
    public Text pointsText;
    public Text nextLevel;
    public Color goodColor;
    public Color badColor;
    public GameObject badResult;
    public GameObject goodResult;
    public GameObject greatResult;

    public void OnOkClicked()
    {
        GameController.OnEndConfirm();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        var lastResult  = gc.ResultController.LastResult;
        var best = gc.ResultController.GetBestResultResult(lastResult.levelId);

        string str_id = "Level:" + lastResult.levelId;
        string time = "Time:" + lastResult.levelTime.ToString("00.00");
        string str_best = " Best:" + best.GetBestStars();
        string points = "Points:" + gc.ResultController.PointsToSpend + "(+" + lastResult.addPoints + ")";
        string stars = "Cookies:" + gc.ResultController.StarsCollected + "(+" + lastResult.addStars + ")";


        greatResult.gameObject.SetActive(false);
        goodResult.gameObject.SetActive(false);

        if (lastResult.addStars >= 4)
        {
            if (lastResult.addStars == 5)
            {
                stars = "Great";
                greatResult.gameObject.SetActive(true);
            }
            else
            {
                stars = "Good";
                greatResult.gameObject.SetActive(true);

            }
            nextLevel.color = goodColor;
        }
        else
        {
            goodResult.gameObject.SetActive(true);
            stars = "Try again";
            nextLevel.color = badColor;

        }
        TimeText.text = time;
        pointsText.text = points;
        nextLevel.text = stars;
        string nn = "\n";
        cureentResult.text = str_id;
        gc.ResultController.Save();
        if (GameController.ResultController.ShowAds())
        {
            if (Advertisement.isReady())
            {
                Advertisement.Show();
            }
        }
    }

    public void OnFBClicked()
    {
        GameController.faceBook.SendImage();
    }

    public void OnRetryClicked()
    {
        var lastResult = GameController.ResultController.LastResult;
        GameController.StartGame(lastResult.levelId);
    }
}

