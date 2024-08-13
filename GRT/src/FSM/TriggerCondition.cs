using System;

namespace GRT.FSM
{
    public class TriggerCondition<T> : ICondition where T : IEquatable<T>
    {
        private T _trigger;

        public T Expected { get; set; }
        public T Default { get; set; }

        /// <summary>
        /// OK属性每调用一次都会清空触发器，重复调用请重新调用Trigger方法传入
        /// </summary>
        bool ICondition.OK
        {
            get
            {
                T temp = _trigger;
                _trigger = Default; // 这里只要调用一次之后就会归位

                return temp.Equals(Expected);
            }
        }

        internal void Trigger(T trigger) => _trigger = trigger;
    }

    public static class TriggerConditionExtension
    {
        public static void Trigger<T>(this FiniteStateMachine fsm, T trigger) where T : IEquatable<T>
        {
            fsm.CurrentOrSelf.Trigger(trigger);
        }

        internal static void Trigger<T>(this IState state, T trigger) where T : IEquatable<T>
        {
            foreach (var transition in state.Transitions)
            {
                transition.Trigger(trigger);
            }
        }

        internal static void Trigger<T>(this ITransition transition, T trigger) where T : IEquatable<T>
        {
            foreach (var condition in transition.Conditions)
            {
                if (condition is TriggerCondition<T> conditionT)
                {
                    conditionT.Trigger(trigger);
                }
                else if (condition is LogicalCondition logicalCondition)
                {
                    logicalCondition.Trigger(trigger);
                }
            }
        }

        internal static void Trigger<T>(this LogicalCondition condition, T trigger) where T : IEquatable<T>
        {
            if (condition.A is LogicalCondition logicalConditionA)
            {
                logicalConditionA.Trigger(trigger);
            }
            else if (condition.A is TriggerCondition<T> triggerConditionA)
            {
                triggerConditionA.Trigger(trigger);
            }

            if (condition.B is LogicalCondition logicalConditionB)
            {
                logicalConditionB.Trigger(trigger);
            }
            else if (condition.B is TriggerCondition<T> triggerConditionB)
            {
                triggerConditionB.Trigger(trigger);
            }
        }
    }
}