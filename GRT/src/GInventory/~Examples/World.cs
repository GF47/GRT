using GRT.GInventory;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.InventoryUI
{
    public class World : MonoBehaviour, IWorld
    {
        [SerializeField] private List<BaseWorldObject> _templates;

        private IWorld _world;

        private void Awake()
        {
            _world = new BaseWorldImpl { _templates = _templates };
        }

        public void OnStackPutDown(IOwner owner, IStack stack) => _world.OnStackPutDown(owner, stack);

        public void OnStackSpawn(IOwner owner, IStack stack) => _world.OnStackSpawn(owner, stack);

        private class BaseWorldImpl : BaseWorld<BaseWorldObject>
        {
            internal List<BaseWorldObject> _templates;

            public override BaseWorldObject Instantiate(IStack stack)
            {
                var template = _templates.Find(wo => wo.name == stack.Model);
                if (template != null)
                {
                    var go = Object.Instantiate(template.gameObject);
                    go.SetActive(true);
                    go.transform.position = Random.insideUnitSphere;

                    var wo = go.GetComponent<BaseWorldObject>();
                    wo.Stack = stack;
                    return wo;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}