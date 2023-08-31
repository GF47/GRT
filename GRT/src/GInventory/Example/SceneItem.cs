using UnityEngine;

namespace GRT.GInventory.Example
{
    public class SceneItem : MonoBehaviour, IInventoryItem
    {
        public IStack Stack { get; private set; }

        public void SetStack(IStack stack)
        {
            if (Stack != null) { Stack.Quantity.Changing -= Quantity_Changing; }
            Stack = stack;
            Stack.Quantity.Changing += Quantity_Changing;

            transform.localScale = (1f+0.1f*Stack.Quantity.Value)*Stack.GetScale();
        }

        private void Quantity_Changing(IStack stack, int nv, int ov)
        {
            transform.localScale = (1f + 0.1f * nv) * stack.GetScale();
        }

        private void OnDestroy()
        {
            if (Stack != null)
            {
                Stack.Quantity.Changing -= Quantity_Changing;
                Stack = null;
            }
        }
    }
}