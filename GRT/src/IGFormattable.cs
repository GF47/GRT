using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GRT
{
    public interface IGFormattable
    {
        IDictionary<string, IGStringifiable> Tags { get; }

        IList<(string, string)> Template { get; }

        void SetValue(string tag, string value);

        string Format();
    }

    public interface IGStringifiable
    {
        string Stringify();
    }

    public static class GFormattableUtils
    {
        public static List<(string, string)> ParseFormattedString(this string template) =>
            new List<(string, string)>(ParseFormattableStringImpl(template));

        private static IEnumerable<(string, string)> ParseFormattableStringImpl(this string template)
        {
            int a = -1;
            int r = a + 1;
            int i = 0;
            while (i < template.Length)
            {
                if (a < 0)
                {
                    if (template[i] == '{')
                    {
                        var length = i - r;
                        if (length > 0)
                        {
                            yield return (null, template.Substring(r, length));
                        }

                        a = i; // 开始配对
                        r = i + 1;
                    }
                }
                else
                {
                    if (template[i] == '}')
                    {
                        var length = i - a - 1;
                        if (length > 0)
                        {
                            yield return (template.Substring(a + 1, length), null);
                        }

                        a = -1; // 配对完毕
                        r = i + 1;
                    }
                }

                i++;
            }

            if (i - r > 0)
            {
                yield return (null, template.Substring(r, i - r));
            }
        }

        public static void SetValueImpl(this IGFormattable formattable, string tag, string value, string name = null)
        {
            var template = formattable.Template;
            if (template != null)
            {
                for (int i = 0; i < template.Count; i++)
                {
                    var pair = template[i];
                    if (pair.Item1 == tag)
                    {
                        template[i] = (tag, value);
                        return;
                    }
                }
            }

            Debug.LogWarning($"{name ?? string.Empty}:\nthere is not a tag named {tag} in the formattable");
        }

        public static string FormatImpl(this IGFormattable formattable)
        {
            var template = formattable.Template;
            if (template != null)
            {
                var builder = new StringBuilder();
                foreach (var pair in template)
                {
                    builder.Append(pair.Item2);
                }
                return builder.ToString();
            }

            return null;
        }

        public static void UpdateImpl(this IGFormattable formattable, Action<string> callback)
        {
            var template = formattable.Template;
            if (template != null)
            {
                foreach (var pair in formattable.Tags)
                {
                    formattable.SetValue(pair.Key, pair.Value.Stringify());
                }
            }

            callback?.Invoke(formattable.Format());
        }

        public static string SimpleFormat(this string template, ICollection<KeyValuePair<string, object>> dict)
        {
            var sb = new StringBuilder(256);
            foreach (var (tag, value) in ParseFormattableStringImpl(template))
            {
                if (tag == null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        sb.Append(value);
                    }
                }
                else
                {
                    foreach (var pair in dict)
                    {
                        if (pair.Key == tag && pair.Value != null)
                        {
                            sb.Append(pair.Value.ToString());
                            break;
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}