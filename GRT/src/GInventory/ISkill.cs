namespace GRT.GInventory
{
    public interface ISkill
    {
        string Name { get; }
        string Description { get; }
        bool IsReady { get; }

        void Invoke(IStack stack);
    }
}