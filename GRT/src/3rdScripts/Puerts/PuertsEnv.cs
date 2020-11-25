#define USE_PUERTS
#undef USE_PUERTS

#if USE_PUERTS
using Puerts;
using System;

namespace GRT._3rdScripts.Puerts
{
    public class PuertsEnv : IEnvironment<JsEnv>
    {
        public JsEnv Env { get; private set; }

        public event Action<JsEnv> OnStart;
        public event Func<JsEnv> Constructor;

        public bool CanUpdate { get; set; }

        public void OnDestroy()
        {
            Env.Dispose();
        }

        public void Start()
        {
            Env = Constructor();
            OnStart?.Invoke(Env);
            OnStart = null;
        }

        public void Update()
        {
            if (CanUpdate) { Env.Tick(); }
        }
    }
}
#endif
