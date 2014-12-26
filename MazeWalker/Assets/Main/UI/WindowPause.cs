
using UnityEngine.UI;

public class WindowPause : BaseWindow
{

    public void OnUnPause()
    {
        GameController.UnPause();
    }

    public void OnExit()
    {
        GameController.EndGame(false);
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
    }
}

