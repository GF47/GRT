namespace GRT.GInventory
{
    public interface ISkill
    {
        string Name { get; }

        string Description { get; }

        bool IsReady { get; }

        /// <summary>
        /// 触发方式
        /// </summary>
        ITrigger Trigger { get; }

        void Invoke(IOwner owner, IStack stack);
    }
}
