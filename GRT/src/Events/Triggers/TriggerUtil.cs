﻿using System;

namespace GRT.Events.Triggers
{
    public static class TriggerUtil
    {
        // public static T CreateTrigger<T, V>()
        //     where T: TriggerDecorator
        //     where V : ITrigger
        // {
        // }

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