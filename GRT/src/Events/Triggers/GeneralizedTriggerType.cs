namespace GRT.Events.Triggers
{
    public enum GeneralizedTriggerType
    {
        None = 0,
        Off = 1 << 0,
        On = 1 << 1,
        Keep = 1 << 2,
        OneShot = On | Off,
    }
}