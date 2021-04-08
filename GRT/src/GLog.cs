/***************************************************************
 * @File Name       : GFDebug
 * @Author          : GF47
 * @Description     : 一个简单的Log
 * @Date            : 2017/5/16/星期二 10:11:56
 * @Edit            : none
 **************************************************************/

#define FPS
#undef FPS

using System;
using System.Collections;
using UnityEngine;

namespace GRT
{
    public class GLog : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _instance = RootGameObject.AddComponent<GLog>();
            _infos = new string[4];
        }

        private static GLog _instance;

        private static string[] _infos;
        private static int _current;

        public static Vector2 Size = new Vector2(Screen.width / 2f, Screen.height / 16f);
        public static Vector2 Pos = new Vector2(16f, 16f);

        public static int Capacity
        {
            get => _infos.Length;
            set
            {
                if (value == 0 || value == _infos.Length) { return; }

                Array.Resize(ref _infos, value);
            }
        }

        public static bool Enabled { get => _instance.enabled; set => _instance.enabled = value; }

        public static void Log(object o, float time = -1f)
        {
            _current++;
            if (_current >= Capacity) { _current = 0; }

            _infos[_current] = o.ToString();
            Enabled = true;

            if (time > 0f)
            {
                _instance.StartCoroutine(__Timer(time, () => Enabled = false));
            }
        }

        private static IEnumerator __Timer(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

#if FPS

        private static bool _showFPS = true;
        private static float _duration;
        private static int _frameCount;
        private static float _fps;

        public static float FPSRefreshDelta = 2f;
        public static Vector2 FPSSize = new Vector2(Screen.width / 4f, Screen.height / 16f);

        public static bool ShowFPS
        {
            get => _showFPS;
            set
            {
                if (_showFPS != value)
                {
                    _showFPS = value;
                    if (_showFPS)
                    {
                        _duration = 0f;
                        _frameCount = 0;
                    }
                }
            }
        }

        void Update()
        {
            if (_showFPS)
            {
                _frameCount += 1;
                _duration += Time.unscaledDeltaTime;

                if (_duration > FPSRefreshDelta)
                {
                    _fps = _frameCount / _duration;
                    _duration = 0f;
                    _frameCount = 0;
                }
            }
        }

#endif

        private static int GetPrevious(int index)
        {
            index--;
            if (index < 0)
            {
                index += Capacity;
            }
            return index;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this) // 防止多个实例
            {
                Debug.LogWarning($"Do not init another {nameof(GLog)}");
                Destroy(this);
            }
        }

        private void OnGUI()
        {
            Rect r = new Rect(Pos.x, Pos.y, Size.x, Size.y);
            for (int i = 0, cursor = _current; i < Capacity; i++, cursor = GetPrevious(cursor), r = new Rect(r.x, r.y + Size.y, r.width, r.height))
            {
                if (_infos != null)
                {
                    if (!string.IsNullOrEmpty(_infos[cursor])) { GUI.Label(r, _infos[cursor]); }
                }
            }
#if FPS
            if (_showFPS)
            {
                GUI.Label(new Rect(Screen.width - FPSSize.x - Pos.x, Pos.y, FPSSize.x, FPSSize.y), _fps.ToString("F2"));
            }
#endif
        }
    }
}