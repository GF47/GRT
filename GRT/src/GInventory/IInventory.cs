namespace GRT.GInventory
{
    public interface IInventory
    {
        void OnStackSpawn(IOwner owner, IStack stack);

        void OnStackDestroy(IStack stack);

        IStack In(IStack stack);

        IStack Out(IStack stack);
    }
}