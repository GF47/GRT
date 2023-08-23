using UnityEngine;

namespace GRT.GInventory.Example
{
    public class Shoot : ISkill
    {
        public string Name => "射击";

        public string Description => "使用选中物体射击";

        public bool IsReady => true;

        public void Invoke(IStack stack)
        {
            var newStack = stack?.Transfer(Scene.Instance);

            if (Scene.Instance.Stacks.TryGetValue(newStack, out var itemOut) && itemOut is SceneItem item)
            {
                var camera = Camera.main.transform;
                item.transform.position = camera.position + camera.up;
                if (item.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    rigidbody.AddForce(20 * camera.forward, ForceMode.Impulse);
                }
            }
        }
    }
}