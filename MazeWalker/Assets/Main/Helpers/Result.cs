
using System;
using UnityEngine;

public class Result
{
    public const string RESULT_SAVE = "save_key";
    public const char DELEMITER = ';';
    public const char DELEMITER_HIGH = ':';
    public float levelTime;
    public int levelId;
    public ControlType controlType;
   // public int curStars;
   // public int totalStars;

    public Result(float levelTime, int levelId, ControlType controlType)
    {
        this.levelTime = levelTime;
        this.levelId = levelId;
        this.controlType = controlType;
        // this.curStars = curStars;
        // this.totalStars = totalStars;
    }

    public Result(string s)
    {
        var r = s.Split(DELEMITER);
        levelId = Convert.ToInt32(r[0]);
        levelTime =  Convert.ToSingle(r[1]);
        //Debug.Log("gg  " + r[2]);
        controlType = (ControlType)Enum.Parse(typeof(ControlType), r[2]);
        // totalStars = Convert.ToInt32(r[3]);
//        Debug.Log("result load " + levelId);
    }

    public override string ToString()
    {
        return "Id:" + levelId + "\n time:" + levelTime + "\n controlType:" + controlType ;
    }

    public string ToStringAlternative()
    {
        return "Id:" + levelId + "\t " + levelTime.ToString("##.#") + "s. \t " + controlType ;
    }

    public void Save()
    {
        string allres = "";
        string r = levelId + DELEMITER.ToString() + levelTime + DELEMITER + controlType + DELEMITER_HIGH;
        if (PlayerPrefs.HasKey(RESULT_SAVE))
        {
            allres = PlayerPrefs.GetString(RESULT_SAVE);
            allres += r;
        }
       // Debug.Log("result allres " + allres);
        PlayerPrefs.SetString(RESULT_SAVE, allres);
    }
}

