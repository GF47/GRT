namespace GRT.Events.Triggers
{
    public interface ITrigger
    {
        GeneralizedTriggerType Type { get; }
        GnityEvent Event { get; }
    }
}