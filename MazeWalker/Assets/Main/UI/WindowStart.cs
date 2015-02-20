using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : BaseWindow
{
    public Text resultsText;
   // public Text currentLevelField;
    //public InputField randomNumberText;
    //public Text selectedLevel;
    //public Toggle randomButton;
    private int curLevel = 1;
    public CurrentResult uiResultWindow;

    public void OnStartClicked()
    {
        GameController.StartGame(curLevel);//randomButton.isOn?-1:Convert.ToInt32(randomNumberText.text));
    }

    public void OnUpLevel()
    {
        if (curLevel < GameController.ResultController.lastLevelNumber)
            curLevel++;
        UpdateCurTevelField();
    }

    public void OnDownLevel()
    {
        if (curLevel > 1)
            curLevel--;
        UpdateCurTevelField();
    } 

    private void UpdateCurTevelField()
    {
       // currentLevelField.text = curLevel + "";
        Result r = GameController.ResultController.GetBestResultResult(curLevel);
        int diff = GameController.ResultController.GetMazeSize(curLevel);
        uiResultWindow.SetResult(r, curLevel, diff);
        /*
        if (r != null)
            selectedLevel.text = r.ToString();
        else
            selectedLevel.text = "No Result";
         */
    }

    public void OnExitClick(){
        Application.Quit();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        curLevel = gc.ResultController.lastLevelNumber;
        UpdateCurTevelField();
        //curLevel = Convert.ToInt32(currentLevelField.text);
        try
        {

            var results = gc.LoadResults();
            //string r = results.Aggregate("", (current, result) => current + (result.ToStringAlternative() + "\n"));
            resultsText.text = gc.ResultController.GetOverview();
        }
        catch (Exception)
        {
            gc.CleatResults();
            resultsText.text = "Results \n was \n cracked";
        }
    }

    public void OnBorderClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.border);
    }

    public void OnSwipeClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.swipe);
    }
    /*
    public void OnLevelNumberChange1(string ss)
    {
        randomButton.isOn = (ss.Length == 0);
    }

    public void OnRandomButtonToggle(Toggle val)
    {
        if (val.isOn)
        {
            randomNumberText.text = "";
        }
        else
        {
           // randomNumberText.text = "1";
        }
    }
    */
    public void OnGyroClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.gyroscope);
    }
}

