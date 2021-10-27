using System;

namespace GRT.Updater
{
    /// <summary> 更新方式 </summary>
    public enum UpdateType
    {
        ///<summary> 逐帧 </summary>
        Frame,

        /// <summary> 逐物理帧 </summary>
        FixedFrame,

        /// <summary> 逐帧之后 </summary>
        AfterFrame,

        /// <summary> 自定义帧速率 </summary>
        CustomFrame
    }

    public interface IUpdater
    {
        /// <summary> 更新方式 </summary>
        UpdateType Type { get; }

        /// <summary> 更新状态 </summary>
        bool IsAlive { get; }

        /// <summary> 更新开始 </summary>
        void Start();

        /// <summary> 更新 </summary>
        /// <param name="delta">时间间隔</param>
        void Update(float delta);

        /// <summary> 更新结束 </summary>
        void Stop();
    }
}