using System;

namespace GRT.GEvents.Triggers
{
    public static class TriggerUtil
    {
        // public static T CreateTrigger<T, V>()
        //     where T: TriggerDecorator
        //     where V : ITrigger
        // {
        // }

        public static bool HasInnerTrigger<T, TI>(this ITrigger<T> trigger, out TI innerTrigger) where TI : ITrigger<T>
        {
            var current = trigger;
            if (current is TI inner)
            {
                innerTrigger = inner;
                return true;
            }

            while (current is TriggerDecorator<T> decorator)
            {
                current = decorator.InnerTrigger;
                if (current is TI inner_)
                {
                    innerTrigger = inner_;
                    return true;
                }
            }

            innerTrigger = default;
            return false;
        }

        public static bool HasInnerTrigger<T>(this ITrigger<T> trigger, Type type, out ITrigger<T> innerTrigger)
        {
            var current = trigger;
            var currentType = current.GetType();
            if (type.IsAssignableFrom(currentType))
            {
                innerTrigger = current;
                return true;
            }

            while (current is TriggerDecorator<T> decorator)
            {
                current = decorator.InnerTrigger;
                currentType = current.GetType();
                if (type.IsAssignableFrom(currentType))
                {
                    innerTrigger = current;
                    return true;
                }
            }

            innerTrigger = null;
            return false;
        }
    }
}