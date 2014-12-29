
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : BaseWindow
{
    public Text resultsText;
    public InputField randomNumberText;
    public Toggle randomButton;

    public void OnStartClicked()
    {
        GameController.StartGame(randomButton.isOn?-1:Convert.ToInt32(randomNumberText.text));
    }

    void Awake()
    {
        randomNumberText.onValueChange.AddListener((string ss) => OnLevelNumberChange1(randomNumberText.text));
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        var results = gc.LoadResults();
        string r = results.Aggregate("", (current, result) => current + (result.ToStringAlternative() + "\n"));
        resultsText.text = r;
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
            randomNumberText.text = "1";
        }
    }

    public void OnGyroClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.gyroscope);
    }
}

