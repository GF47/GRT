using System;

namespace GRT.GInventory.Quantifiables
{
    public class Singleton : IQuantifiable
    {
        public string Type => Keywords.COUNT;

        public int Value => 1;

        public int Max => 1;

        public event Action<IStack, int, int> Changing;
        // {
        //     add { /* warning */ }
        //     remove { /* warning */ }
        // }

        public IQuantifiable Clone(int count) => new Singleton();

        public void SetValue(IStack stack, int count)
        {
            if (count <= 0)
            {
                Changing?.Invoke(stack, 0, 1);
                stack.Destroy();
            }
        }
    }
}