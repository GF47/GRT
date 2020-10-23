using System;

namespace GRT.Updater
{
    public class PerFixedFrameUpdateNode : NormalUpdateNode
    {
        public override UpdateType Type => UpdateType.PerFixedFrame;

        public PerFixedFrameUpdateNode(Action<float> callback) : base(callback) { }
    }
}
