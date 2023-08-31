using UnityEngine;
using UnityEngine.UI;

namespace GRT.GInventory.Example
{
    public class PlayerItem : MonoBehaviour, IInventoryItem
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _count;

        public IStack Stack { get; private set; }

        public void SetStack(IStack stack)
        {
            if (Stack != null) { Stack.Quantity.Changing -= Quantity_Changing; }
            Stack = stack;
            Stack.Quantity.Changing += Quantity_Changing;

            _image.sprite = ExampleSettings.Instance.GetIcon(Stack.Definition.GetIcon());
            _name.text = Stack.Definition.Name;
            _count.text = Stack.Quantity.Value.ToString();
        }

        private void Quantity_Changing(IStack stack, int nv, int ov) => _count.text = nv.ToString();

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                if (Stack?.Inventory is Player player)
                {
                    player.CurrentStack = Stack;
                }
            });
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