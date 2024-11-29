using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Editor.GPie
{
    [CreateAssetMenu(fileName = "GPie")]
    public class GPiePanel : ScriptableObject
    {
        #region instance

        public static GPiePanel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = AssetDatabase.LoadAssetAtPath<GPiePanel>("Assets/GPie.asset");
                    if (_instance == null)
                    {
                        _instance = CreateInstance<GPiePanel>();

                        var debug = new UnityAction<string>(s => Debug.Log($"GF47: {s}"));

                        _instance._items = new BranchedItem[]
                        {
                            new BranchedItem("GF47", new StringUEvent(debug), "Hello World!", new SealedItem[]
                            {
                                new SealedItem("Hello", new StringUEvent(debug), "Hello"),
                                new SealedItem("World", new StringUEvent(debug), "World"),
                            }),
                            new BranchedItem("Play Default Scene", new StringUEvent(_instance.PlayDefaultScene)),
                        };
                    }
                }

                return _instance;
            }
        }

        private static GPiePanel _instance;

        [ContextMenu("Use this GPie Panel")]
        private void SetInstance()
        {
            _instance = this;
            Debug.LogWarning($"[{AssetDatabase.GetAssetPath(this)}] was used");
        }

        #endregion instance

        private static IItem _current;

        private static BranchedItem _origin;

        private static Vector2 _originPosition;

        [ClutchShortcut("g_pie_panel", KeyCode.G, defaultShortcutModifiers: ShortcutModifiers.None, displayName = "GPie Panel")]
        private static void Init(ShortcutArguments args)
        {
            if (args.stage == ShortcutStage.Begin)
            {
                Clear();

                SceneView.duringSceneGui += Draw;
                _originPosition = GetMousePosition(true);
            }
            else if (args.stage == ShortcutStage.End)
            {
                if (_current != null && _current.UEvent != null)
                {
                    _current.UEvent.Invoke(_current.Argument);
                }

                Clear();
            }
        }

        private static void Draw(SceneView sceneView)
        {
            Handles.BeginGUI();
            {
                _current = null;

                var h = EditorGUIUtility.singleLineHeight + 4f;
                var halfh = EditorGUIUtility.singleLineHeight / 2f;

                if (_origin != null)
                {
                    DrawItem(new Rect(_originPosition.x - 64f, _originPosition.y - halfh, 128f, h), _origin);
                    DrawItems(_origin.Submenu);
                }
                else
                {
                    DrawItem(new Rect(_originPosition.x - halfh, _originPosition.y + halfh, h, h), null);
                    DrawItems(Instance._items);
                }
            }
            Handles.EndGUI();
        }

        private static void DrawItems(IItem[] items)
        {
            var rot = 0f;
            var delta = 2f * Mathf.PI / Mathf.Max(items.Length, 1f);

            foreach (var item in items)
            {
                var w = 128f;
                var h = EditorGUIUtility.singleLineHeight;
                var x = _originPosition.x + 128f * Mathf.Cos(rot) - w * Mathf.Sin(rot / 2f);
                var y = _originPosition.y - 128f * Mathf.Sin(rot) - h * (rot / 2f > Mathf.PI ? 1f : 0.5f);

                var rect = new Rect(x, y, w, h);
                DrawItem(rect, item);

                rot += delta;
            }
        }

        private static void DrawItem(Rect rect, IItem item)
        {
            if (item == null)
            {
                GUI.Label(rect, string.Empty, EditorStyles.toggle);
            }
            else if (rect.Contains(GetMousePosition(false)))
            {
                var defaultColor = GUI.color;
                GUI.color = Color.yellow;
                {
                    GUI.Label(rect, item.Name, EditorStyles.miniButton);
                }
                GUI.color = defaultColor;

                _current = item;

                if (_current != _origin && _current is BranchedItem branchedItem && branchedItem.Submenu != null && branchedItem.Submenu.Length > 0)
                {
                    _origin = branchedItem;
                    _originPosition = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
                }
            }
            else
            {
                GUI.Label(rect, item.Name, EditorStyles.miniButton);
            }
        }

        private static void Clear()
        {
            SceneView.duringSceneGui -= Draw;

            _current = null;
            _origin = null;
        }

        private static Vector2 GetMousePosition(bool shit)
        {
            var pos = Event.current.mousePosition;
            if (shit)
            {
                pos.y -= 64f; // 40 for editor top menu and 24 for center box
            }
            return pos;
        }

        #region items

        [Serializable]
        private class StringUEvent : UnityEvent<string>
        {
            public StringUEvent(UnityAction<string> action)
            {
                if (action != null)
                {
                    AddListener(action);
                }
            }

            public StringUEvent()
            { }
        }

        private interface IItem
        {
            string Name { get; }

            StringUEvent UEvent { get; }

            string Argument { get; }
        }

        [Serializable]
        private class BranchedItem : SealedItem
        {
            [SerializeField] private SealedItem[] _submenu;

            public SealedItem[] Submenu => _submenu;

            public BranchedItem(string name, StringUEvent uEvent, string argument = null, SealedItem[] submenu = null) : base(name, uEvent, argument)
            {
                _submenu = submenu;
            }
        }

        [Serializable]
        private class SealedItem : IItem
        {
            [SerializeField] private string _name;
            [SerializeField] private string _argument;
            [SerializeField] private StringUEvent _uEvent;

            public string Name => _name;
            public string Argument => _argument;
            public StringUEvent UEvent => _uEvent;

            public SealedItem(string name, StringUEvent uEvent, string argument = null)
            {
                _name = name;
                _uEvent = uEvent;
                _argument = argument;
            }
        }

        [SerializeField] private MonoScript[] _scripts;
        [SerializeField] private BranchedItem[] _items;

        #endregion items

        /***************示例：执行菜单项*******************************/

        public void ExecuteMenuItem(string menu) => EditorApplication.ExecuteMenuItem(menu);

        public void OpenScene(string path) => UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);

        public void AddScene(string path) => UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path, UnityEditor.SceneManagement.OpenSceneMode.Additive);

        /***************示例：打开发布设置中的第一个场景*******************************/

        #region examples

        public void PlayDefaultScene(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                if (EditorBuildSettings.scenes.Length > 0)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
                }
            }
            else
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);
            }

            EditorApplication.EnterPlaymode();
        }

        /***************示例：静态类方法***************************************/

        public void InvokeMonoScriptMethod(string arg)
        {
            if (arg.CanBeSplitBy('.', out var cName, out var mCall))
            {
                var script = _scripts?.FindExt(s => s.GetClass().Name == cName);
                if (script != null)
                {
                    var mName = mCall.Substring(0, mCall.IndexOf('('));
                    var mArg = mCall.SubStringInBrackets();
                    if (mArg.Length > 0 && mArg[0] == '"')
                    {
                        mArg = mArg.SubStringInBrackets('"', '"');
                    }
                    var class_ = script.GetClass();
                    var method = class_.GetMethod(mName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (string.IsNullOrEmpty(mArg))
                    {
                        Debug.LogWarning($"call method {cName}.{mName}()");
                        method?.Invoke(null, null);
                    }
                    else
                    {
                        Debug.LogWarning($"call method {cName}.{mName}(\"{mArg}\")");
                        method?.Invoke(null, new object[] { mArg });
                    }
                }
            }
        }

        #endregion examples
    }
}