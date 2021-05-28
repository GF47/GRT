using System;

namespace GRT
{
    public static class GRandom
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public static int Get()
        {
            return _random.Next();
        }
    }
}