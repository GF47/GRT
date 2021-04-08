namespace GRT.Tween
{
    public enum Ease
    {
        Linear,
        QuadIn, QuadOut, QuadInOut,
        CubicIn, CubicOut, CubicInOut,
        QuartIn, QuartOut, QuartInOut,
        BackIn, BackOut, BackInOut,
        BounceIn, BounceOut, BounceInOut,
        // ElasticIn, ElasticOut, ElasticInOut,
    }

    public enum Loop
    {
        Once = 0,
        Loop,
        PingPong,
        PingPongOnce
    }

    public enum Direction
    {
        Forward = 1,
        Backward = -1,
        Toggle = 0
    }
}
