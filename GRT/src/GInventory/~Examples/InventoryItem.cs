using GRT;
using GRT.GInventory;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.InventoryUI
{
    public class InventoryItem : MonoBehaviour, IInventoryItem
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _count;

        private int _quantity;

        public string Name { get => _name.text; set => _name.text = value; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public int Quantity
        {
            get => _quantity; set
            {
                _quantity = value;
                _count.text = _quantity.ToString();
            }
        }

        public void OnQuantityValueChange(IStack stack, int value, int oldValue) => Quantity = value;

        /**************************************************************/

        private static Pool<InventoryItem> _pool;
        private static GameObject _template;

        public static void Init(GameObject template)
        {
            _template = template;

            _pool = new Pool<InventoryItem>();

            _pool.Initialize(5, () =>
            {
                return Instantiate(_template).GetComponent<InventoryItem>();
            });

            _pool.Getting += item =>
            {
                item.gameObject.SetActive(true);
            };

            _pool.Releasing += item =>
            {
                item.gameObject.SetActive(false);
            };
        }

        public static InventoryItem Get() => _pool.Get();

        public static void Release(InventoryItem item) => _pool.Release(item);
    }
}