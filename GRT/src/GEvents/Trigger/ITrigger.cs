namespace GRT.GEvents.Triggers
{
    public interface ITrigger<T>
    {
        GeneralizedTriggerType Type { get; }
        GnityEvent<T> Event { get; }
    }
}