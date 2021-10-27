using System;

namespace GRT.Updater
{
    public class AfterFrameUpdater : NormalUpdater
    {
        public AfterFrameUpdater(Action<float> updating) : base(updating)
        {
        }

        public override UpdateType Type => UpdateType.AfterFrame;
    }
}