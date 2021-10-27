using System;

namespace GRT.Updater
{
    public class FrameUpdater : NormalUpdater
    {
        public FrameUpdater(Action<float> updating) : base(updating)
        {
        }

        public override UpdateType Type => UpdateType.Frame;
    }
}