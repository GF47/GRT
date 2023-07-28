using System;

namespace GRT.Events.Triggers
{
    public static class TriggerUtil
    {
        // public static T CreateTrigger<T, V>()
        //     where T: TriggerDecorator
        //     where V : ITrigger
        // {
        // }

        public static bool HasInnerTrigger<T>(this ITrigger trigger, out T innerTrigger) where T : ITrigger
        {
            var current = trigger;
            if (current is T inner)
            {
                innerTrigger = inner;
                return true;
            }

            while (current is TriggerDecorator decorator)
            {
                current = decorator.InnerTrigger;
                if (current is T inner_)
                {
                    innerTrigger = inner_;
                    return true;
                }
            }

            innerTrigger = default;
            return false;
        }

        public static bool HasInnerTrigger(this ITrigger trigger, Type type, out ITrigger innerTrigger)
        {
            var current = trigger;
            var currentType = current.GetType();
            if (type.IsAssignableFrom(currentType))
            {
                innerTrigger = current;
                return true;
            }

            while (current is TriggerDecorator decorator)
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