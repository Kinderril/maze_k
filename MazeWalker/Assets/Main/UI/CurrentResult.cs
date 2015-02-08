using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CurrentResult : MonoBehaviour {

    public Text levelNumber;
    public StarContainer star;
    private List<StarContainer> stars = new List<StarContainer>();
    public float step = 40f;
    public int starCount = 5;
    public float y_offset = 0f;

    void Awake()
    {
        float leftOffset = -step * (starCount - 1) / 2;
        for (int i = 0; i < starCount; i++)
        {
            StarContainer s = Instantiate(star);
            s.transform.parent = this.transform;
            stars.Add(s);
            s.transform.localPosition = new Vector3(leftOffset + i * step, y_offset, 0);
            s.Close();
        }

    }
    public void SetResult(Result r,int levl)
    {
       // Debug.Log("rrr " + levl);
        levelNumber.text = "Level:" + levl;
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
