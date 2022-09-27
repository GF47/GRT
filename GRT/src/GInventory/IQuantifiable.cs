using System;

namespace GRT.GInventory
{
    /// <summary>
    /// 量化数据，堆叠数量或者耐久等
    /// </summary>
    public interface IQuantifiable
    {
        /// <summary>
        /// 值改变时触发事件，第一个int为新值，第二个int为旧值
        /// </summary>
        event Action<IStack, int, int> ValueChanging;

        /// <summary>
        /// 量化数据的类型
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 实际数据
        /// </summary>
        int Value { get; }

        /// <summary>
        /// 最大值
        /// </summary>
        int Max { get; }

        void SetValue(IStack stack, int value);

        /// <summary>
        /// 复制一个指定值的相同量化数据
        /// </summary>
        IQuantifiable Clone(int value);

        // void ClearValueChangingEvents();
    }
}