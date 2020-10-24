//************************************************************//
//      Author      :       GF47
//      DataTime    :       2014/3/2 3:20:27
//      Edited      :       2014/3/2 3:20:27
//************************************************************//

namespace GRT.Tween
{
    /// <summary>
    /// 运算结果
    /// </summary>
    internal struct Factor
    {
        public Factor(float percent, bool finished)
        {
            factor = percent;
            isFinished = finished;
        }
        public float factor;
        public bool isFinished;
    }
}
