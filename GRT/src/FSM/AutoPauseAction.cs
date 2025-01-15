using System.Collections;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class AutoPauseAction : IAction, IResetable, IEnumerable<IResetable>
    {
        private int _conditionsIsOK = -1;

        public ICollection<ICondition> Conditions { get; }
        public ICollection<IAction> Actions { get; }

        public bool Completed
        {
            get
            {
                foreach (var action in Actions)
                {
                    if (!action.Completed) { return false; }
                }

                return true;
            }
        }

        public AutoPauseAction(ICollection<ICondition> conditions, ICollection<IAction> actions)
        {
            Conditions = conditions;
            Actions = actions;
        }

        public void Invoke()
        {
            Check();

            if (_conditionsIsOK < 1)
            {
                return;
            }
            else if (_conditionsIsOK == 1)
            {
                if (Actions != null)
                {
                    foreach (var action in Actions)
                    {
                        action.Start();
                    }
                }
                _conditionsIsOK = 2;
            }
            else if (_conditionsIsOK > 1)
            {
                if (Actions != null)
                {
                    foreach (var action in Actions)
                    {
                        if (!action.Completed)
                        {
                            action.Invoke();
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            _conditionsIsOK = -1;

            this.DeepReset(false);
        }

        public void Start()
        { }

        /// <summary>
        /// check 后一定会有 _conditionsIsOK = 0 或 = 1 或 = 2
        /// </summary>
        private void Check()
        {
            if (_conditionsIsOK == -1)
            {
                _conditionsIsOK = 1; // 已验证且正确
            }

            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    if (condition != null && !condition.OK) { _conditionsIsOK = 0; } // 已验证且不正确
                }
            }
        }

        public IEnumerator<IResetable> GetEnumerator() => Util.CombineIResetables(Conditions, Actions);

        IEnumerator IEnumerable.GetEnumerator() => Util.CombineIResetables(Conditions, Actions);
    }
}