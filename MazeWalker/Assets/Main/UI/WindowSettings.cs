using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WindowSettings : BaseWindow {

    public ToggleGroup ballToggle1;
    public ToggleGroup trailToggle;
    public override void Init(GameController gc)
    {
        base.Init(gc);
        //ballToggle1.SetAllTogglesOff();

        int i = 0;
        var togglsList = ballToggle1.GetComponentsInChildren<Toggle>();
        foreach(var item in  togglsList)
        {
            var pints2open = GameController.settings.pointsToOpen * i;
            bool isAvalable = (pints2open <= GameController.ResultController.StarsCollected);
            item.isOn = (i == GameController.settings.BallIndex && isAvalable);
            item.interactable = isAvalable;
            var textField = item.GetComponentInChildren<Text>();
            if (textField != null)
            {
                textField.gameObject.SetActive(!isAvalable);
                if (!isAvalable)
                {
                    textField.text = (pints2open - GameController.ResultController.StarsCollected).ToString();
                }
            }
            i++;
        }
        i = 0;
        var togglsList2 = trailToggle.GetComponentsInChildren<Toggle>();
        foreach (var item in togglsList2)
        {
            var pints2open = GameController.settings.pointsToOpenTrails * i;
            bool isAvalable = (pints2open <= GameController.ResultController.StarsCollected);
            item.isOn = (i == GameController.settings.TrailIndex && isAvalable);
            item.interactable = isAvalable;
            var textField = item.GetComponentInChildren<Text>();
            if (textField != null)
            {
                textField.gameObject.SetActive(!isAvalable);
                if (!isAvalable)
                {
                    textField.text = (pints2open - GameController.ResultController.StarsCollected).ToString();
                }
            }
            i++;
        }
    }

    public void SetBallColor1(Toggle x)
    {
        if (x.isOn)
        {
            GameController.settings.BallIndex = x.GetComponent<IntBeh>().Index;
        }
    }
    public void SetTrail(Toggle x)
    {
        if (x.isOn)
        {
            GameController.settings.TrailIndex = x.GetComponent<IntBeh>().Index;
        }
    }

    public void OnExit()
    {
        GameController.OnMainMenu();
    }
}
