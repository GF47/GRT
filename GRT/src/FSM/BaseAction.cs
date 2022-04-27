using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRT.FSM
{
    public abstract class BaseAction : IAction
    {
        public virtual bool Completed { get; protected set; }

        public abstract void Start();

        public abstract void Invoke();

        public virtual void Reset()
        {
            Completed = false;
        }
    }
}
