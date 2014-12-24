
using UnityEngine.UI;

public class WindowEnd : BaseWindow
{
    public Text endMsg;

    public void OnOkClicked()
    {
        gameController.OnEndConfirm();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        endMsg.text = "Your results is:" + "\n" + gc.curStars + "/" + gc.maxStars + "\n time is: " + gc.GetTime();
    }
}

