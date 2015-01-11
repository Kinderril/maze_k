
using UnityEngine;
public class GameUtils
{
    public static Vector3 GetV3BySide(Side s)
    {
        Vector3 v = Vector3.zero;
        switch (s)
        {
            case Side.down:
                v = new Vector3(0,180,0);
                break;
            case Side.left:
                v = new Vector3(0, 90, 0);
                break;
            case Side.right:
                v = new Vector3(0, -90, 0);
                break;
        }
        return v;
    }
}

