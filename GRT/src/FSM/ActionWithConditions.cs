using System.Collections;
using System.Collections.Generic;

namespace GRT.FSM
{
    public class ActionWithConditions : IAction, IEnumerable<IResetable>
    {
        public ICollection<ICondition> Conditions { get; }

        public ICollection<IAction> Actions { get; }

        public bool Repeat { get; set; } = true;

        public bool Completed
        {
            get
            {
                if (_conditionIsOK > 0) // 已验证并正确
                {
                    foreach (var action in Actions)
                    {
                        if (!action.Completed) { return false; }
                    }

                    return true;
                }
                else if (_conditionIsOK == 0) // 已验证但错误
                {
                    return !Repeat;
                }
                else // 未验证
                {
                    return false;
                }
            }
        }

        private int _conditionIsOK = -1;

        public ActionWithConditions(ICollection<ICondition> conditions, ICollection<IAction> actions)
        {
            Conditions = conditions;
            Actions = actions;
        }

        public void Invoke()
        {
            if (_conditionIsOK < 1)
            {
                if (Repeat)
                {
                    Check();
                }
            }
            else if (_conditionIsOK == 1)
            {
                if (Actions != null)
                {
                    foreach (var action in Actions)
                    {
                        action.Start();
                    }
                }

                _conditionIsOK = 2;
            }
            else if (_conditionIsOK > 1)
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
            _conditionIsOK = -1; // 未验证状态

            this.DeepReset(false);
        }

        public void Start() => Check();

        /// <summary>
        /// check 后一定会有 _conditionIsOk = 0 或 = 1
        /// </summary>
        private void Check()
        {
            _conditionIsOK = 1; // 已验证并正确
            if (Conditions != null)
            {
                foreach (var condition in Conditions)
                {
                    if (!condition.NullOrOK()) { _conditionIsOK = 0; } //已验证并错误
                }
            }
        }

        public IEnumerator<IResetable> GetEnumerator() => Util.CombineIResetables(Conditions, Actions);

        IEnumerator IEnumerable.GetEnumerator() => Util.CombineIResetables(Conditions, Actions);
    }
}