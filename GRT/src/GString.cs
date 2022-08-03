using System.Collections.Generic;

namespace GRT
{
    public static class GString
    {
        public static bool CanBeSplitBy(this string str, char c, out string left, out string right)
        {
            var i = str.IndexOf(c);
            if (i > -1)
            {
                left = str.Substring(0, i);
                right = str.Substring(i + 1);
                return true;
            }
            else
            {
                left = null;
                right = str;
                return false;
            }
        }

        public static List<KeyValuePair<string, string>> ParseTaggedString(this string template)
        {
            var result = new List<KeyValuePair<string, string>>();

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
                            result.Add(new KeyValuePair<string, string>(null, template.Substring(r, length)));
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
                            result.Add(new KeyValuePair<string, string>(template.Substring(a + 1, length), null));
                        }

                        a = -1; // 配对完毕
                        r = i + 1;
                    }
                }

                i++;
            }

            if (i - r > 0)
            {
                result.Add(new KeyValuePair<string, string>(null, template.Substring(r, i - r)));
            }

            return result;
        }
    }
}