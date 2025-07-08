using System;

namespace GRT
{
    public interface ILinkable<T> where T : struct
    {
        T Content { get; set; }

        ILinkable<T> Left { get; set; }
        ILinkable<T> Right { get; set; }

        bool SetContentAlone(T content);
    }

    public class Linkable<T> : ILinkable<T> where T : struct, IEquatable<T>
    {
        private T _content;

        public T Content
        {
            get => _content; set
            {
                if (SetContentAlone(value))
                {
                    this.ForLeft(SyncContent);
                    this.ForRight(SyncContent);
                }
            }
        }

        ILinkable<T> ILinkable<T>.Left { get; set; }
        ILinkable<T> ILinkable<T>.Right { get; set; }

        public event Action<T, T> Changing;

        /// <summary>
        /// 赋值时，对新值进行预处理
        /// </summary>
        public Func<T, T> Preprocess;

        public bool SetContentAlone(T content)
        {
            var newContent = Preprocess == null ? content : Preprocess(content);

            if (!_content.Equals(newContent))
            {
                var oldContent = _content;
                _content = newContent;
                Changing?.Invoke(_content, oldContent);
                return true;
            }

            return false;
        }

        public void SetContentByForce(T content)
        {
            var newContent = Preprocess == null ? content : Preprocess(content);

            var oldContent = _content;
            _content = newContent;
            Changing?.Invoke(_content, oldContent);

            this.ForLeft(SyncContent);
            this.ForRight(SyncContent);
        }

        private bool SyncContent(ILinkable<T> linkable)
        {
            linkable.SetContentAlone(_content);
            return true; // 返回值为是否继续
        }
    }

    public static class LinkableExtensions // <T, LT> where T : struct where LT : ILinkable<T>
    {
        public static bool IsLinked<T>(this ILinkable<T> self, ILinkable<T> others)where T : struct
        {
            var isLinked = false;
            ForEach(self, linkable =>
            {
                if (linkable == others) { isLinked = true; }
                return !isLinked;
            });

            return isLinked;
        }

        public static void Link<T>(this ILinkable<T> self, ILinkable<T> other) where T : struct
        {
            if (self.IsLinked(other)) { return; }

            var rightEnd = self.GetRightEnd();
            var leftEnd = other.GetLeftEnd();
            rightEnd.Right = leftEnd;
            leftEnd.Left = rightEnd;
        }

        public static void Unlink<T>(this ILinkable<T> self) where T : struct
        {
            var left = self.Left;
            var right = self.Right;

            if (left != null) { left.Right = right; }
            if (right != null) { right.Left = left; }

            self.Left = null;
            self.Right = null;
        }

        public static void ForLeft<T>(this ILinkable<T> self, Func<ILinkable<T>, bool> func) where T : struct
        {
            if (func == null) { return; }

            var n = self.Left;
            while (n != null)
            {
                if (n == self || !func.Invoke(n)) { return; }
                n = n.Left;
            }
        }

        public static void ForRight<T>(this ILinkable<T> self, Func<ILinkable<T>, bool> func) where T : struct
        {
            if (func == null) { return; }

            var n = self.Right;
            while (n != null)
            {
                if (n == self || !func.Invoke(n)) { return; }
                n = n.Right;
            }
        }

        public static void ForEach<T>(this ILinkable<T> self, Func<ILinkable<T>, bool> func) where T : struct
        {
            if (func == null || !func.Invoke(self)) { return; }

            var n = self.Left;
            while (n != null)
            {
                if (n == self || !func.Invoke(n)) { return; }
                n = n.Left;
            }

            n = self.Right;
            while (n != null)
            {
                if (n == self || !func.Invoke(n)) { return; }
                n = n.Right;
            }
        }

        public static ILinkable<T> GetLeftEnd<T>(this ILinkable<T> self) where T : struct
        {
            var n = self;
            while (n.Left != null)
            {
                if (n == self) { return n.Right; }
                n = n.Left;
            }
            return n;
        }

        public static ILinkable<T> GetRightEnd<T>(this ILinkable<T> self) where T : struct
        {
            var n = self;
            while (n.Right != null)
            {
                if (n == self) { return n.Left; } // 防止环形链表
                n = n.Right;
            }
            return n;
        }
    }
}
