namespace GRT.GInventory
{
    public class BaseOwner : IOwner
    {
        public IInventory Inventory { get; set; }

        public IStack Current { get; set; }

        public void PickUp(IStack stack)
        {
            if (stack != null)
            {
                stack.PickUp(this);

                var stackIn = Inventory.In(stack);

                if (stackIn.Skills != null)
                {
                    foreach (var skill in stackIn.Skills)
                    {
                        skill.Trigger.GetContext = () => (this, Inventory, Current);
                    }
                }
            }
        }

        public void PutDown(IStack stack)
        {
            if (stack != null)
            {
                var stackOut = Inventory.Out(stack);

                stackOut.PutDown(this);

                // if (stack.Skills != null)
                // {
                //     foreach (var skill in stack.Skills)
                //     {
                //         skill.Trigger.GetContext = null;
                //     }
                // }
            }
        }
    }
}