
using UnityEngine.UI;

public class WindowEnd : BaseWindow
{
    public Text endMsg;

    public void OnOkClicked()
    {
        GameController.OnEndConfirm();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        endMsg.text = gc.GetLastResult().ToString();
        gc.SaveResult();
    }
}

