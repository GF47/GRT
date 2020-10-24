//************************************************************//
//      Author      :       GF47
//      DataTime    :       2013/9/17 星期二 13:42:08
//      Edited      :       2013/9/17 星期二 13:42:08
//************************************************************//

using System;
using UnityEngine;

namespace GRT.Tween
{
    /// <summary>
    /// Author          :GF47
    /// DataTime        :2013/9/17 星期二 13:42:08
    /// Introduction    :对float值的缓动
    /// </summary>
    public class Tween<T> : MonoBehaviour, IPercent
    {
        public float Percent
        {
            get { return _percent; }
            set
            {
                _percent = value;
                realValue = setValue(_percent);
            }
        }

        private float _percent;
        protected T realValue;

        public T from;
        public T to;

        public Func<float, T> setValue;
    }
}
