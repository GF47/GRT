using System.Collections.Generic;

namespace GRT
{
    public static class GString
    {
        public static bool CanBeSplitBy(this string str, char c, out string left, out string right, bool asLeft = false, bool last = false)
        {
            var i = last ? str.LastIndexOf(c) : str.IndexOf(c);
            if (i > -1)
            {
                left = str.Substring(0, i);
                right = str.Substring(i + 1);
                return true;
            }
            else
            {
                if (asLeft)
                {
                    left = str;
                    right = null;
                }
                else
                {
                    left = null;
                    right = str;
                }
                return false;
            }
        }

        public static IEnumerable<(string, string)> ParseTaggedString(this string template)
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
                        var length = i - r;
                        if (length > 0)
                        {
                            var equal = template.IndexOf('=', r, length);
                            if (equal != -1)
                            {
                                yield return (template.Substring(r, equal - r), template.Substring(equal + 1, i - equal - 1));
                            }
                            else
                            {
                                yield return (template.Substring(r, length), null);
                            }
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
    }
}