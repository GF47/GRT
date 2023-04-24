using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Editor
{
    [CreateAssetMenu(fileName = "GPie")]
    public class GPiePanel : ScriptableObject
    {
        private static bool _isActive;
        private static Vector2 _position;
        private static Item[] _current;
        private static Item2[] _currentSub;

        [Shortcut("g_pie_panel", KeyCode.G, displayName = "GPie Panel")]
        private static void Init()
        {
            if (!_isActive)
            {
                Clear();

                var pie = AssetDatabase.LoadAssetAtPath<GPiePanel>("Assets/GRT/GPie.asset");
                if (pie == null)
                {
                    _current = new Item[]
                    {
                        new Item()
                        {
                            name = "GF47",
                            children= new Item2[]
                            {
                                new Item2() { name = "Hello" },
                                new Item2() { name = "World" },
                            }
                        },
                        new Item() { name="Unity" },
                    };
                }
                else
                {
                    _current = pie._items;
                }

                _isActive = true;
                SceneView.duringSceneGui += OnSceneView;
                _position = GetCurrentMousePosition(true);
            }
        }

        private static void OnSceneView(SceneView view)
        {
            if (Event.current.keyCode == KeyCode.Escape)
            {
                Clear();
                return;
            }

            Handles.BeginGUI();
            {
                GUI.Box(new Rect(_position.x, _position.y, 20f, 20f), "O");

                if (_currentSub != null && _currentSub.Length > 0)
                {
                    DrawItem2();
                }
                else
                {
                    DrawItem();
                }
            }
            Handles.EndGUI();
        }

        private static void DrawItem()
        {
            var rot = 0f;
            var delta = 2f * Mathf.PI / Mathf.Max(_current.Length, 1f);

            // var pos = Event.current.mousePosition;
            // var indicator = (int)((Mathf.Atan2(pos.y - _position.y, _position.x - pos.x) + Mathf.PI) / delta + 0.5f);

            var items = _current;
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                var w = 128f;
                var h = 24f;
                var x = _position.x + 128f * Mathf.Cos(rot) - w * Mathf.Sin(rot / 2f);
                var y = _position.y - 128f * Mathf.Sin(rot) - h * (rot / 2f > Mathf.PI ? 1f : 0.5f);

                // var color = GUI.color;
                // if (indicator == i) { GUI.color = Color.yellow; }
                {
                    if (GUI.Button(new Rect(x, y, w, h), item.name))
                    {
                        item.action?.Invoke();

                        if (item.children != null && item.children.Length > 0)
                        {
                            _position = GetCurrentMousePosition(false);
                            _currentSub = item.children;
                        }
                        else
                        {
                            Clear();
                        }
                    }
                }
                // GUI.color = color;

                rot += delta;
            }
        }

        private static void DrawItem2()
        {
            var rot = 0f;
            var delta = 2f * Mathf.PI / Mathf.Max(_currentSub.Length, 1f);

            // var pos = Event.current.mousePosition;
            // var indicator = (int)((Mathf.Atan2(pos.y - _position.y, _position.x - pos.x) + Mathf.PI) / delta + 0.5f);

            var items = _currentSub;
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                var w = 128f;
                var h = 24f;
                var x = _position.x + 128f * Mathf.Cos(rot) - w * Mathf.Sin(rot / 2f);
                var y = _position.y - 128f * Mathf.Sin(rot) - h * (rot / 2f > Mathf.PI ? 1f : 0.5f);

                // var color = GUI.color;
                // if (indicator == i) { GUI.color = Color.yellow; }
                {
                    if (GUI.Button(new Rect(x, y, w, h), item.name))
                    {
                        item.action?.Invoke();
                        Clear();
                    }
                }
                // GUI.color = color;

                rot += delta;
            }
        }

        private static void Clear()
        {
            _isActive = false;

            SceneView.duringSceneGui -= OnSceneView;
            _current = null;
            _currentSub = null;
        }

        private static Vector2 GetCurrentMousePosition(bool shit)
        {
            var pos = Event.current.mousePosition;
            if (shit)
            {
                pos.y -= 64f; // 40 for editor top menu and 24 for center box
            }
            return pos;
        }

        /**************************************************************/

        [Serializable]
        private class Item
        {
            public string name;
            public UnityEvent action;

            public Item2[] children;
        }

        [Serializable]
        public class Item2
        {
            public string name;
            public UnityEvent action;
        }

        [SerializeField]
        private Item[] _items;
    }
}