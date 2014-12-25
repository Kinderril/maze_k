using UnityEngine;
using System.Collections;

public class StarContainer : MonoBehaviour
{

    public GameObject closeStar;
    public GameObject openStar;
    private bool isOpen = false;
    
    public bool Open()
    {
        if (isOpen)
            return false;
        closeStar.gameObject.SetActive(false);
        openStar.gameObject.SetActive(true);
        isOpen = true;
        return true;
    }

    public void Close()
    {
        closeStar.gameObject.SetActive(true);
        openStar.gameObject.SetActive(false);
        isOpen = false;
    }

}
