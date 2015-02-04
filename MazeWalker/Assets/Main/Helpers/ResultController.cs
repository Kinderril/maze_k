﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultController
{
    private List<Result> results = new List<Result>();
  //  public Dictionary<int, ResultLevel> list = new Dictionary<int, ResultLevel>(); 

    public const string RESULT_SAVE = "save_key";
    public const char DELEMITER_HIGH = ':';
    public const string RESULT_SAVE_STARS2SPEND = "RESULT_SAVE_STARS2SPEND";
    public const string RESULT_SAVE_STARSCOLLECTED = "RESULT_SAVE_STARSCOLLECTED";
    public int lastLevelNumber = 1;
    private int starsCollected;
    private int starsToSpend;




    private Result lastResult;
    public Result LastResult
    {
        get { return lastResult; }
    }

    public ResultController()
    {
        try
        {
            LoadResults();
        }
        catch (Exception)
        {
            
        }
    }
    public int StarsCollected
    {
        get { return starsCollected; }
        set
        {
            starsCollected = value;
            PlayerPrefs.SetInt(RESULT_SAVE_STARSCOLLECTED, starsCollected);
            CalcPosibleLevel();
        }
    }

    private void CalcPosibleLevel()
    {
        if (starsCollected == 0)
            lastLevelNumber = 1;
        else
        {
            lastLevelNumber = (int) (Mathf.Pow(Mathf.Log(starsCollected + 4), 3)/3f);
          //  Debug.Log("lastLevelNumber " + lastLevelNumber + "  " + starsCollected);
        }
    }

    private int NextLevelStarsNeed(int lvl)
    {
        return (int)Mathf.Pow((float)Math.E, Mathf.Pow(lvl*3f, 1f/3f)) + 1 - 4;
    }


    public int StarsToSpend
    {
        get { return starsToSpend; }
        set {
            starsToSpend = value;
            PlayerPrefs.SetInt(RESULT_SAVE_STARS2SPEND, starsToSpend);
        }
    }
    private void LoadResults()
    {
        
        var list = new List<Result>();
        if (PlayerPrefs.HasKey(RESULT_SAVE))
        {
            string s = PlayerPrefs.GetString(RESULT_SAVE);
            list.AddRange(from item in s.Split(DELEMITER_HIGH) where item.Length > 5 select new Result(item));
        }
        results = list;
        starsToSpend = PlayerPrefs.GetInt(RESULT_SAVE_STARS2SPEND, 0);
        starsCollected = PlayerPrefs.GetInt(RESULT_SAVE_STARSCOLLECTED, 0);
        CalcPosibleLevel();
    }

    public void AddResult(float levelTime, int levelId, ControlType controlType, int starsCount)
    {
        StarsCollected += starsCount;
        StarsToSpend += starsCount;

        var r = results.FirstOrDefault(x => x.levelId == levelId);
        if (r == null)
        {
            var result = new Result(levelId);
            result.AddResultLevel(controlType, levelTime, starsCount);
            results.Add(result);
            r = result;
        }
        else
        {
            r.AddResultLevel(controlType, levelTime, starsCount);
        }
        var lresult = new Result(levelId);
        lresult.AddResultLevel(controlType, levelTime, starsCount);
        lastResult = lresult;
    }


    public void Save()
    {
        string ss = results.Aggregate("", (current, result) => current + (result.GetSaveString() + DELEMITER_HIGH));
        PlayerPrefs.SetString(RESULT_SAVE, ss);
    }

    public List<Result> Load()
    {
        LoadResults();
        return results;
    }

    public void Clear()
    {
        PlayerPrefs.SetString(RESULT_SAVE, "");
        PlayerPrefs.SetInt(RESULT_SAVE_STARS2SPEND, 0);
        PlayerPrefs.SetInt(RESULT_SAVE_STARSCOLLECTED, 0);
    }

    public float GetMidByControlType(ControlType ct)
    {
        float m = 0;
        int i = 0;
        foreach (var result in results.Where(result => result.list.ContainsKey(ct)))
        {
            m += result.list[ct];
            i++;
        }
        return m/i;
    }

    public int GetMazeSize()
    {
        return (int)(Mathf.Pow(Mathf.Log(lastLevelNumber + 3), 2) * 1.3f + 14);
    }
    public Result GetBestResultResult(int levelId)
    {
        return results.FirstOrDefault(x => x.levelId == levelId);
    }

    public string GetOverview()
    {
        string ss = "Stars:" + starsCollected;
        ss += "\n Points:" + starsToSpend;
        ss += "\n Level:" + (1+lastLevelNumber);
        ss += "\n Next:" + NextLevelStarsNeed(lastLevelNumber + 1);
        return ss;

    }

}

