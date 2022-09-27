using System;

namespace GRT.GInventory.Quantifiables
{
    public class Singleton : IQuantifiable
    {
        public string Type => Count.TYPE;

        public int Value => 1;

        public int Max => 1;

        /// <summary>
        /// 不可能发生改变, 什么事都不做
        /// </summary>
        public event Action<IStack, int, int> ValueChanging
        {
            add
            {
                // warn
            }
            remove
            {
                // warn
            }
        }

        // /// <summary>
        // /// 不可能发生改变, 什么事都不做
        // /// </summary>
        // public void ClearValueChangingEvents()
        // {
        //     // warn
        // }

        public IQuantifiable Clone(int value) => new Singleton();

        public void SetValue(IStack stack, int value)
        {
            if (value <= 0)
            {
                stack.Destroy();
            }
        }
    }
}