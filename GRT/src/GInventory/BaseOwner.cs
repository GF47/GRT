using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GInventory
{
    public class BaseOwner<T> : IOwner where T : Component, IWorldObject<T>
    {
        private IStack _currentStack;

        public event Action<IStack, IStack> CurrentStackChanging;

        public IInventory Inventory { get; set; }

        public IWorld World { get; set; }

        public IStack CurrentStack
        {
            get => _currentStack; set
            {
                if (_currentStack != value)
                {
                    var oldStack = _currentStack;
                    _currentStack = value;

                    if (oldStack != null)
                    {
                        foreach (var skill in oldStack.Skills)
                        {
                            if (skill.Trigger != null)
                            {
                                skill.Trigger.Enabled = false;
                            }
                        }
                    }

                    if (_currentStack != null)
                    {
                        foreach (var skill in _currentStack.Skills)
                        {
                            if (skill.Trigger != null)
                            {
                                skill.Trigger.Enabled = true;
                            }
                        }
                    }

                    CurrentStackChanging?.Invoke(_currentStack, oldStack);
                }
            }
        }

        public void Spawn(ICollection<IStack> stacks)
        {
            foreach (var stack in stacks)
            {
                stack.Definition.Spawning += World.OnStackSpawn;
                stack.Definition.PuttingDown += World.OnStackPutDown;

                stack.Definition.Spawning += Inventory.OnStackSpawn;
                stack.Definition.Destroying += Inventory.OnStackDestroy;

                stack.Definition.Destroying += (s) =>
                {
                    if (s == CurrentStack)
                    {
                        CurrentStack = null;
                    }
                };

                foreach (var skill in stack.Definition.Skills)
                {
                    if (skill.Trigger != null)
                    {
                        skill.Trigger.GetContext = () => (this, CurrentStack);
                    }
                }

                stack.Spawn(this);
            }
        }

        public void PickUp(IStack stack)
        {
            if (stack != null)
            {
                stack.PickUp(this);

                Inventory.In(stack);
            }
        }

        public void PutDown(IStack stack)
        {
            if (stack != null)
            {
                var stackOut = Inventory.Out(stack);

                stackOut.PutDown(this);
            }
        }
    }
}