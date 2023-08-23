namespace GRT.GInventory
{
    public abstract class IDGenerator
    {
        public static IDGenerator Instance { get; protected set; }

        public abstract int Generate();
    }
}