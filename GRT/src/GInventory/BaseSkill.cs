namespace GRT.GInventory
{
    public abstract class BaseSkill : ISkill
    {
        private ITrigger _trigger;

        public string Name { get; set; }

        public string Description { get; set; }

        public ITrigger Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                _trigger.Triggering += Invoke;
            }
        }

        public abstract void Invoke(IOwner owner, IStack stack);
    }
}