
public class ObstacleParameters
{

    public int w;
    public int h;
    public IntPos enter;
    public IntPos exit;
    public Side rotation;
    public bool withStar = false;

    public ObstacleParameters(int w, int h, IntPos enter, IntPos exit, Side rotation, bool withStar)
    {
        this.w = w;
        this.h = h;
        this.enter = enter;
        this.exit = exit;
        this.rotation = rotation;
        this.withStar = withStar;
    }
}

