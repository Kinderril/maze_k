
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

    private Result lastResult;
    public Result LastResult
    {
        get { return lastResult; }
    }

    public ResultController()
    {
      //  Clear();
        //PlayerPrefs.SetString(RESULT_SAVE, "dsfdsfdsfds");
        try
        {
            LoadResults();
        }
        catch (Exception)
        {
            
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
         
    }

    public void AddResult(float levelTime, int levelId, ControlType controlType)
    {
        var r = results.FirstOrDefault(x => x.levelId == levelId);
        if (r == null)
        {
            var result = new Result(levelId);
            result.AddResultLevel(controlType,levelTime);
            results.Add(result);
            r = result;
        }
        else
        {
            r.AddResultLevel(controlType, levelTime);
        }
        lastResult = r;
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
}

