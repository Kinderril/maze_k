using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Result
{
    public Dictionary<ControlType, float> list = new Dictionary<ControlType, float>();
    public const char DELEMITER = ';';
    public int levelId;
 
    public Result(int level)
    {
        levelId = level;
    }

    public Result(string load)
    {
        UnityEngine.Debug.Log(load);
        string[] ss =  load.Split(DELEMITER);
        levelId = Convert.ToInt32(ss[0]);
        for (int i = 1; i < ss.Length-1; i+=2)
        {
            UnityEngine.Debug.Log(i + "   " + ss[i]);
            var a = (ControlType)Enum.Parse(typeof(ControlType), ss[i]);
            float t = Convert.ToSingle(ss[i + 1]);
            list.Add(a,t);
        }
    }

    public void AddResultLevel(ControlType control,float time)
    {
        if (!list.ContainsKey(control))
            list.Add(control, time);
        else
        {
            list[control] = Mathf.Min(time, list[control]);
        }
    }

    public override string ToString()
    {
        string ss = "Id:" + levelId + "\n";
        ss = list.Aggregate(ss, (current, f) => current + ("time:" + f.Value + "\n Control:" + f.Key));
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

