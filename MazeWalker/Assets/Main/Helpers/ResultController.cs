
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultController
{
    private List<Result> results = new List<Result>();
  //  public Dictionary<int, ResultLevel> list = new Dictionary<int, ResultLevel>(); 

    public const string RESULT_SAVE = "save_key";
    public const string POSSIBLE_LEVEL = "POSSIBLE_LEVEL";
    public const char DELEMITER_HIGH = ':';
    public const string RESULT_SAVE_STARS2SPEND = "RESULT_SAVE_STARS2SPEND";
    public const string RESULT_SAVE_STARSCOLLECTED = "RESULT_SAVE_STARSCOLLECTED";
    public int lastLevelNumber = 1;
    private int starsCollected;
    private int _pointsToSpend;




    private LastResult lastResult;
    public LastResult LastResult
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
            //CalcPosibleLevel();
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


    public int PointsToSpend
    {
        get { return _pointsToSpend; }
        set {
            _pointsToSpend = value;
            PlayerPrefs.SetInt(RESULT_SAVE_STARS2SPEND, _pointsToSpend);
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
        _pointsToSpend = PlayerPrefs.GetInt(RESULT_SAVE_STARS2SPEND, 0);
        starsCollected = PlayerPrefs.GetInt(RESULT_SAVE_STARSCOLLECTED, 0);
        lastLevelNumber = PlayerPrefs.GetInt(POSSIBLE_LEVEL, 1);
        //CalcPosibleLevel();
    }

    public void AddResult(float levelTime, int levelId, ControlType controlType, int starsCount)
    {
        if (levelId == lastLevelNumber)
        {
            if (starsCount >= 4)
            {
                lastLevelNumber++;
            }
        }
        PointsToSpend += starsCount;
        var bsetResult = GetBestResultResult(levelId);
        int taken = starsCount;
        if (bsetResult != null)
        {
            int addStars = bsetResult.GetBestStars();
            starsCount = Mathf.Clamp(starsCount - addStars, 0, 9999);
        }
        StarsCollected += starsCount;


        var r = results.FirstOrDefault(x => x.levelId == levelId);
        if (r == null)
        {
            var result = new Result(levelId);
            result.AddResultLevel(controlType, levelTime, taken);
            results.Add(result);
            r = result;
        }
        else
        {
            r.AddResultLevel(controlType, levelTime, taken);
        }

        lastResult = new LastResult(levelId, levelTime, starsCount, taken);
    //    lastResult.AddResultLevel(controlType, levelTime, starsCount, taken);
    }


    public void Save()
    {
        string ss = results.Aggregate("", (x, result) => x + (result.GetSaveString() + DELEMITER_HIGH));
        PlayerPrefs.SetString(RESULT_SAVE, ss);
        PlayerPrefs.SetInt(POSSIBLE_LEVEL, lastLevelNumber);
    }

    public List<Result> Load()
    {
        LoadResults();
        return results;
    }

    public void Clear()
    {
        PlayerPrefs.SetInt(POSSIBLE_LEVEL, 1);
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

    public int GetMazeSize(int number)
    {
        int r = (int)(Mathf.Pow(Mathf.Log(number + 3), 2) * 1.65f + 14);
     //   Debug.Log("Maze zise " + r + "   " + number);
        return r;
    }
    public Result GetBestResultResult(int levelId)
    {
        return results.FirstOrDefault(x => x.levelId == levelId);
    }

    public string GetOverview()
    {
        string ss = "Stars: " + starsCollected;
        ss += "\nPoints: " + _pointsToSpend;
       // ss += "\nLevel:" + (lastLevelNumber);
       // ss += "\nNext:" + NextLevelStarsNeed(lastLevelNumber + 1);
        return ss;

    }

}

