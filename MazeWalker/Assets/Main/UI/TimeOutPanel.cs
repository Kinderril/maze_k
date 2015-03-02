using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class TimeOutPanel : MonoBehaviour
{
    public Text label;
    public Text main;
    private Action callback;

    public void Init(GameController gc, Action callback)
    {
        bool isBonus = gc.maze.IsBonusLevel();
        Debug.Log("isBonus " + isBonus + "  ");
        if (isBonus)
        {
            main.text = "Bonus level";
        }
        this.callback = callback;
        StartCoroutine(DoCount(3));
    }

    public IEnumerator DoCount(int c)
    {
        Debug.Log("DoCount " + c + "  ");
        if (c == 0)
        {
            callback();
            StopAllCoroutines();
        }
        else
        {
            label.text = c.ToString("0");
            yield return new WaitForSeconds(1f);
            StartCoroutine(DoCount(c - 1));
        }
    } 
}

