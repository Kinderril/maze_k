using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class WindowManager
{
    private static List<BaseWindow> allWindows;
    private static BaseWindow curWindow;
    private static GameController gc;

    public static void Init(List<BaseWindow> allWindowsInc, GameController gc)
    {
        allWindows = allWindowsInc;
        WindowManager.gc = gc;
    }

    public static void WindowOn(BaseWindow bw)
    {
        if (curWindow == bw)
            return;
        if (curWindow != null)
        {
            curWindow.Dispose();
            curWindow.gameObject.SetActive(false);
        }
        curWindow = bw;
        bw.gameObject.SetActive(true);
        bw.Init(gc);
        foreach (var item in allWindows.Where(x => x != bw && x != null))
        {
            item.Dispose();
            item.gameObject.SetActive(false);
        }
    }

    public static BaseWindow GetCurrent()
    {
        return curWindow;
    }
}

