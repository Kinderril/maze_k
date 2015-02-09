
using System.Security.Principal;

public class LastResult
{
    public int addStars;
    public int addPoints;
    public int levelId;
    public float levelTime;

    public LastResult(int levelId, float levelTime, int starsCount, int taken)
    {
        this.levelId = levelId;
        this.levelTime = levelTime;
        this.addStars = starsCount;
        this.addPoints = taken;
    }
}

