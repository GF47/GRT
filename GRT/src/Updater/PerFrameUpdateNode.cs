using System;

namespace GRT.Updater
{
    public class PerFrameUpdateNode : NormalUpdateNode
    {
        public override UpdateType Type => UpdateType.PerFrame;

        public PerFrameUpdateNode(Action<float> updating) : base(updating) { }
    }
}
