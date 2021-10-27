using System;

namespace GRT.FSM
{
    public class Transition : ITransition
    {
        protected Func<bool> conditions;

        public Func<bool> Conditions => conditions;

        public bool OK
        {
            get
            {
                var methods = conditions.GetInvocationList();
                for (int i = 0; i < methods.Length; i++)
                {
                    if (!((Func<bool>)methods[i]).Invoke())
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public IState To { get; set; }
    }
}
