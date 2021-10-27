using System;

namespace GRT.Updater
{
    public class FixedFrameUpdater : NormalUpdater
    {
        public FixedFrameUpdater(Action<float> updating) : base(updating)
        {
        }

        public override UpdateType Type => UpdateType.FixedFrame;
    }
}