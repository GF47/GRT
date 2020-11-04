#define USE_PUERTS
#undef USE_PUERTS

#if USE_PUERTS
using Puerts;
using System;
using UnityEngine;

namespace GRT._3rdScripts.Puerts
{
    public class PuertsEnv : MonoBehaviour, IEnvironment
    {
        public JsEnv Env { get; private set; }

        public event Action OnStart;

        public void OnDestroy()
        {
            Env.Dispose();
        }

        public void Start()
        {
            Env = new JsEnv();
            OnStart?.Invoke();
        }

        public void Update()
        {
            Env.Tick();
        }
    }
}
#endif
