using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Result
{
    public Dictionary<ControlType, int> list = new Dictionary<ControlType, int>();
    public const char DELEMITER = ';';
    public int levelId;
    public float time;
 
    public Result(int level)
    {
        levelId = level;
    }

    public Result(string load)
    {
//        UnityEngine.Debug.Log(load);
        string[] ss =  load.Split(DELEMITER);
        levelId = Convert.ToInt32(ss[0]);
        for (int i = 1; i < ss.Length-1; i+=2)
        {
          //  UnityEngine.Debug.Log(i + "   " + ss[i]);
            var a = (ControlType)Enum.Parse(typeof(ControlType), ss[i]);
            int t = Convert.ToInt32(ss[i + 1]);
            list.Add(a,t);
        }
    }

    public int GetBestStars()
    {
        int r = 0;
        foreach (var item in list)
        {
            if (item.Value > r)
                r = (int)item.Value;
        }

        return r;
    }

    public void AddResultLevel(ControlType control, float time, int starsCount )
    {
        if (!list.ContainsKey(control))
            list.Add(control, starsCount);
        else
        {
            list[control] = Mathf.Max(starsCount, list[control]);
        }
        this.time = time;
    }

    public override string ToString()
    {
        string ss = "Level:" + levelId + "\n";
        ss = list.Aggregate(ss, (current, f) => current + ("Stars:" + f.Value + "/5\n " /*+ f.Key*/));
        return ss;
    }

    public string ToStringAlternative()
    {
        string ss = "Id:" + levelId + "\t";
        ss = list.Aggregate(ss, (current, f) => current + ("\n \t" + f.Key + "\t" + f.Value.ToString("00.00")));
        return ss;
    }

    public string GetSaveString()
    {
        string ss = ""+ levelId + DELEMITER;
        ss= list.Aggregate(ss, (current, f) => current + ("" + f.Key + DELEMITER + f.Value + DELEMITER));
        return ss;
    }
}

