using System;

namespace GRT.Updater
{
    public class PerAfterFrameUpdateNode : NormalUpdateNode
    {
        public override UpdateType Type => UpdateType.PerAfterFrame;

        public PerAfterFrameUpdateNode(Action<float> updating) : base(updating) { }
    }
}
