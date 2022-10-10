using System.Collections.Generic;
using UnityEngine;

namespace GRT.Events
{
    public class Blocker
    {
        private readonly Dictionary<int, Vector4> _blocks = new Dictionary<int, Vector4>();

        public int Block(Vector4 area)
        {
            var i = GRandom.Get();
            _blocks.Add(i, area);
            return i;
        }

        public void Unblock(int i) => _blocks.Remove(i);

        public void UpdateBlock(int i, Vector4 area, bool addIfNotContain = false)
        {
            if (addIfNotContain && !_blocks.ContainsKey(i))
            {
                _blocks.Add(i, area);
                return;
            }

            _blocks[i] = area;
        }

        public bool Blocking(Vector2 pos)
        {
            foreach (var pair in _blocks)
            {
                var area = pair.Value;
                if (area[0] < pos.x && pos.x < area[2] && area[1] < pos.y && pos.y < area[3])
                {
                    return true;
                }
            }
            return false;
        }
    }
}