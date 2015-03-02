
using UnityEngine;
using UnityEngine.UI;

public class WindowEnd : BaseWindow
{
    public Text cureentResult;
    public Text TimeText;
    public Text pointsText;
    public Text nextLevel;
    public Color goodColor;
    public Color badColor;

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
        string stars = "Stars:" + gc.ResultController.StarsCollected + "(+" + lastResult.addStars + ")";
        if (best.GetBestStars() >= 4)
        {
            stars = "Great";
            nextLevel.color = goodColor;
        }
        else
        {
            stars = "Try again";
            nextLevel.color = badColor;

        }
        TimeText.text = time;
        pointsText.text = points;
        nextLevel.text = stars;
        string nn = "\n";
        cureentResult.text = str_id;// +nn+ time+nn + stars +nn+ points + nn + str_best;
        //cureentResult.text = "Current:\n" + lastResult.ToString();
       // bestResult.text = "Best:\n" + best.ToString();
        gc.ResultController.Save();
      //  addStars.text = " addStars:" + lastResult.addStars + "  addPoints:" + lastResult.addPoints;
      //  addPoints.text = " GetBestStars:" + best.GetBestStars();
    }

    public void OnFBClicked()
    {
        GameController.faceBook.SendImage();
    }
}

