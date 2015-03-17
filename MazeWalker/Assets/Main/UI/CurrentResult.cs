using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CurrentResult : MonoBehaviour {

    public Text levelNumber;
    public StarContainer star;
    public List<StarContainer> stars = new List<StarContainer>();
    public float step = 40f;
    public int starCount = 5;
    public float y_offset = 0f;
    private bool isInited = false;

    public void SetResult(Result r,int levl,int size)
    {
        if (!isInited)
        {
            foreach (var starContainer in stars)
            {
                starContainer.Close();
            }
            isInited = true;

        }
       // Debug.Log("rrr " + levl);

        levelNumber.text = "Stage:" + levl;// + "("  +size+ ")";
        if (r != null)
        {
            int starsCount = r.GetBestStars();
            float time = r.time;
            for (int i = 0; i < stars.Count; i++)
            {
                if (i < starsCount)
                    stars[i].Open();
                else
                    stars[i].Close();

            }
        }
        else
        {
            foreach (var item in stars)
            {
                item.Close();
            }

        }
    }
}
