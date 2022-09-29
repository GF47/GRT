using GRT;
using GRT.GInventory;
using GRT.GInventory.Quantifiables;
using GRT.GInventory.Triggers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.InventoryUI
{
    public class Owner : MonoBehaviour, IOwner
    {
        private readonly (string, string, string, string, bool, int)[] _data =
        {
            ("Cube AAA", "Cube AAA", "", "Item Cube", true, 3),
            ("Cube BBB", "Cube BBB", "", "Item Cube", false, 1),
            ("Cylinder CCC", "Cylinder CCC", "", "Item Cylinder", false, 1),
            ("Sphere DDD", "Sphere DDD", "", "Item Sphere", true, 4),
        };

        [SerializeField] private Inventory _inventory;
        [SerializeField] private World _world;
        [SerializeField] private InventoryItem _currentItem;

        private BaseOwner<BaseWorldObject> _owner;

        public IInventory Inventory => _inventory;

        public IWorld World => _world;

        private void Start()
        {
            _owner = new BaseOwner<BaseWorldObject>()
            {
                Inventory = _inventory,
                World = _world,
            };
            _owner.CurrentStackChanging += Owner_CurrentStackChanging;

            var stacks = Parse();
            _owner.Spawn(stacks);

            _inventory.Selecting += Inventory_Selecting;
        }

        private void Owner_CurrentStackChanging(IStack _currentStack, IStack oldStack)
        {
            if (_currentStack != null)
            {
                _currentItem.Name = _currentStack.Name;
                _currentItem.Quantity = _currentStack.Quantity.Value;

                _currentStack.Quantity.ValueChanging += Quantity_ValueChanging;
            }

            if (oldStack != null)
            {
                oldStack.Quantity.ValueChanging -= Quantity_ValueChanging;
            }
        }

        private void Quantity_ValueChanging(IStack stack, int nv, int ov)
        {
            if (nv < 1)
            {
                _currentItem.Name = string.Empty;
                _currentItem.Quantity = 0;
            }
            else
            {
                _currentItem.Quantity = stack.Quantity.Value;
            }
        }

        private void Inventory_Selecting(IStack stack, IStack last)
        {
            _owner.CurrentStack = stack;

            _currentItem.Name = _owner.CurrentStack.Name;
            _currentItem.Quantity = _owner.CurrentStack.Quantity.Value;
        }

        private ICollection<IStack> Parse()
        {
            var stacks = new List<IStack>(_data.Length);
            foreach (var (nName, nDesc, nIcon, nModel, nAutoSpawn, nCount) in _data)
            {
                var skill = new TestSkill()
                {
                    Message = GRandom.Get().ToString(),
                    Name = nName + " skill",
                    Description = nDesc + " skill desc",
                    Trigger = new KeyPointerTrigger(KeyCode.F),
                };

                var skills = new ISkill[]
                {
                    skill,
                };

                var definition = new BaseDefinition()
                {
                    Name = nName,
                    Description = nDesc,
                    Icon = nIcon,
                    Model = nModel,
                    Skills = skills,
                };

                var stack = new BaseStack()
                {
                    Definition = definition,
                    Quantity = new Count(nCount),
                    Properties = new Dictionary<string, object>(),
                };
                stack.SetProperty(InventoryKeyword.AUTO_SPAWN, nAutoSpawn);
                stack.SetProperty(InventoryKeyword.POS, Random.insideUnitSphere);

                stacks.Add(stack);
            }

            return stacks;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && Camera.main.Raycast(out var hit))
            {
                if (hit.collider.TryGetComponent<BaseWorldObject>(out var wo))
                {
                    _owner.PickUp(wo.Stack);
                }
            }
        }

        public void PutDownCurrent()
        {
            _owner.PutDown(_owner.CurrentStack);
        }

        internal class TestSkill : BaseSkill
        {
            public string Message { get; set; }

            public override void Invoke(IOwner owner, IStack stack)
            {
                Debug.Log($"{owner} use {stack.Name} and said: {Message}");
            }
        }
    }
}