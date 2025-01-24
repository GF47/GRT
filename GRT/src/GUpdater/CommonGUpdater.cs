using System;

namespace GRT.GUpdater
{
    public class CommonGUpdater : MiniGLifecycleWithTick
    {
        public event Action<IGStartable> StartingOneShot;

        public event Action<IGDisposable> DisposingOneShot;

        public override void GStart()
        {
            base.GStart();
            StartingOneShot?.Invoke(this);
            StartingOneShot = null;
        }

        public override void GDispose()
        {
            base.GDispose();
            DisposingOneShot?.Invoke(this);
            DisposingOneShot = null;
        }

        public void ClearOneShotEvents()
        {
            StartingOneShot = null;
            DisposingOneShot = null;
        }
    }
}