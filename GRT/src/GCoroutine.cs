using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace GRT
{
    public class GCoroutine : MonoBehaviour
    {
        private static GCoroutine _instance;

        private static SynchronizationContext _context;

        public static SynchronizationContext UnityContext => _context;

        public static void Init(GameObject root)
        {
            _instance = root.AddComponent<GCoroutine>();
            _context = SynchronizationContext.Current;

            Debug.Log($"{nameof(GCoroutine)} loaded on {_instance.name}");
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"Do not init another {nameof(GCoroutine)}");
                Destroy(this);
            }
        }

        public static Coroutine YieldThen(YieldInstruction instruction, Action callback) => _instance.StartCoroutine(__YieldThen(instruction, callback));

        public static Coroutine YieldThen(IEnumerator enumerator, Action callback) => _instance.StartCoroutine(__YieldThen(enumerator, callback));

        public static Coroutine Yield(YieldInstruction instruction) => _instance.StartCoroutine(__Yield(instruction));

        public static Coroutine Yield(IEnumerator enumerator) => _instance.StartCoroutine(enumerator);

        public static Coroutine DelayInvoke(Action action, float t) => _instance.StartCoroutine(__YieldThen(new WaitForSecondsRealtime(t), action));

        public static void Stop(Coroutine coroutine) => _instance.StopCoroutine(coroutine);

        public static void Stop(IEnumerator enumerator) => _instance.StopCoroutine(enumerator);

        public static void StopAll() => _instance.StopAllCoroutines();

        private static IEnumerator __Yield(YieldInstruction instruction)
        {
            yield return instruction;
        }

        private static IEnumerator __YieldThen(IEnumerator enumerator, Action callback)
        {
            yield return enumerator;
            callback?.Invoke();
        }

        private static IEnumerator __YieldThen(YieldInstruction instruction, Action callback)
        {
            yield return instruction;
            callback?.Invoke();
        }
    }
}