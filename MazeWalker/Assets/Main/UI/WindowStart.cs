
using System.Linq;
using UnityEngine.UI;

public class WindowStart : BaseWindow
{
    public Text resultsText;

    public void OnStartClicked()
    {
        GameController.StartGame();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        var results = gc.LoadResults();
        string r = results.Aggregate("", (current, result) => current + (result.ToStringAlternative() + "\n"));
        resultsText.text = r;
    }

    public void OnBorderClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.border);
    }

    public void OnSwipeClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.swipe);
    }

    public void OnGyroClicked(bool ok)
    {
        if (ok)
            GameController.ChangeControlType(ControlType.gyroscope);
    }
}

