using System;

namespace GRT.FSM
{
    public class TriggerCondition<T> : ICondition
    {
        /// <summary> 判断两个值是否相等的静态方法 </summary>
        public static Func<T, T, bool> EqualFunc;

        private T _trigger;

        public readonly T expected;
        public readonly T default_;

        /// <summary>
        /// OK属性每调用一次都会清空触发器，重复调用请重新调用Trigger方法传入
        /// </summary>
        bool ICondition.OK
        {
            get
            {
                T temp = _trigger;
                _trigger = default_; // 这里只要调用一次之后就会归位

                if (EqualFunc != null)
                {
                    return EqualFunc(expected, temp);
                }

                // Console.WriteLine($"{GetType()} 的 EqualFunc 方法没有被指定，请手动设定以防止值类型以object方式比较");
                return Equals(expected, temp);
            }
        }

        internal void Trigger(T trigger) => _trigger = trigger;

        public TriggerCondition(T expected, T @default = default)
        {
            this.expected = expected;
            this.default_ = @default;
            this._trigger = @default;
        }
    }

    public static class TriggerConditionExtension
    {
        public static void Trigger<T>(this FiniteStateMachine fsm, T trigger)
        {
            fsm.CurrentState.Trigger(trigger);
        }

        internal static void Trigger<T>(this IState state, T trigger)
        {
            foreach (var transition in state.Transitions)
            {
                transition.Trigger(trigger);
            }
        }

        internal static void Trigger<T>(this ITransition transition, T trigger)
        {
            foreach (var condition in transition.Conditions)
            {
                if (condition is TriggerCondition<T> conditionT)
                {
                    conditionT.Trigger(trigger);
                }
            }
        }
    }
}