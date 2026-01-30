using System.Collections.Generic;

namespace GRT.GPie
{
    public interface IItem
    {
        string Name { get; }

        void Invoke();
    }

    public interface IItemCollection : IItem, IEnumerable<IItem>
    {
    }
}