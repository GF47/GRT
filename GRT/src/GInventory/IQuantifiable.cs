using System;

namespace GRT.GInventory
{
    public interface IQuantifiable
    {
        /// <summary>
        /// 值发生改变时触发事件，第一个int值为新，第二个int值为旧
        /// </summary>
        event Action<IStack, int, int> Changing;

        /// <summary>
        /// 量化数据的类型
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 实际值
        /// </summary>
        int Value { get; }

        /// <summary>
        /// 可取最大值
        /// </summary>
        int Max { get; }

        /// <summary>
        /// 单次转移的量
        /// </summary>
        int Dose { get; }

        void SetValue(IStack stack, int value);

        /// <summary>
        /// 复制一个量化数据，但实际值设为新值
        /// </summary>
        IQuantifiable Clone(int value);
    }
}