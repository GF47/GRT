#define USE_PUERTS

using UnityEngine;

namespace GRT._3rdScripts
{
    public class ScriptsEnvironment
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            RootGameObject.OnGRTInitializing += () =>
            {
#if USE_PUERTS
                var impl = RootGameObject.AddComponent<Puerts.PuertsEnv>();
                Instance = impl;
                Debug.Log($"{nameof(Puerts.PuertsEnv)} loaded on {impl.name}");
#else
#endif

            };
        }


#if USE_PUERTS
        public static Puerts.PuertsEnv Instance { get; private set; }
#else
        public static IEnvironment Instance { get; private set; }
#endif
    }
}
