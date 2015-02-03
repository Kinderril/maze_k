
using UnityEngine.UI;

public class WindowEnd : BaseWindow
{
    public Text cureentResult;
    public Text bestResult;

    public void OnOkClicked()
    {
        GameController.OnEndConfirm();
    }

    public override void Init(GameController gc)
    {
        base.Init(gc);
        var r  = gc.ResultController.LastResult;
        cureentResult.text = "Current:\n"+r.ToString();
        bestResult.text = "Best:\n" + gc.ResultController.GetBestResultResult(r.levelId).ToString();
        gc.ResultController.Save();
    }

    public void OnFBClicked()
    {
        GameController.faceBook.SendImage();
    }
}

