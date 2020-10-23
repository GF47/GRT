/* ****************************************************************
 * @File Name   :   BezierCurve
 * @Author      :   GF47
 * @Date        :   2015/1/16 9:12:38
 * @Description :   创建一条贝塞尔曲线
 * @Edit        :   2015/1/16 9:12:38
 * ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.Geometry.Bezier
{
    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    [Obsolete("请使用[BezierSpline]")]
    public class BezierCurve
    {
        public struct Result
        {
            public Vector3[] array;
            public int length;

            public Vector3 Position { get { return array[0]; } }
            public Vector3 Forward { get { return array[1]; } }

            public Result(IList<Vector3> list)
            {
                array = new Vector3[list.Count];
                for (int i = 0,iMax = array.Length; i < iMax; i++)
                {
                    array[i] = list[i];
                }
                length = list.Count;
            }
        }
        public Vector3 this[int index]
        {
            get
            {
                if (_points == null) return _start;
                if (_points.Count <= index || index < 0) return _start;
                return _points[index] + _start;
            }
            set
            {
                if (_points == null)
                {
                    _points = new List<Vector3>();
                }

                if (index == 0)
                {
                    _start = value;
                }
                if (_points.Count <= index)
                {
                    for (int i = _points.Count; i < index; i++)
                    {
                        _points.Add(Vector3.zero);
                    }
                    _points.Add(value - _start);
                }
                else
                {
                    _points[index] = value - _start;
                }
            }
        }
        public int Count
        {
            get { return _points == null ? 0 : _points.Count; }
            set
            {
                if (_points == null) _points = new List<Vector3>();
                if (_points.Count < value)
                {
                    for (int i = _points.Count; i < value; i++)
                    {
                        _points.Add(Vector3.zero);
                    }
                    return;
                }
                if (_points.Count > value)
                {
                    _points.RemoveRange(value, _points.Count - value);
                }
            }
        }
        private List<Vector3> _points;

        private Vector3 _start;
        public enum LoopType
        {
            Once, Loop, PingPong
        }
        public LoopType loopType = LoopType.Once;

        public BezierCurve(IList<Vector3> list)
        {
            if (list == null || list.Count == 0)
            {
                this[0] = Vector3.zero;
                this[1] = Vector3.zero;
                return;
            }
            if (list.Count == 1)
            {
                this[0] = list[0];
                this[1] = Vector3.zero;
                return;
            }
            for (int i = 0, iMax = list.Count; i < iMax; i++)
            {
                this[i] = list[i];
            }
        }

        public Result GetPoint(float t)
        {
            Result result = new Result(_points);
            GetPointByList(ref result, ClampT(t, loopType));
            result.array[0] += _start;
            return result;
        }

        private static void GetPointByList(ref Result result, float t)
        {
            if (result.length > 2)
            {
                for (int i = 0, imax = result.length - 1; i < imax; i++)
                {
                    result.array[i] = Vector3.Lerp(result.array[i], result.array[i + 1], t);
                }
                result.length--;
                GetPointByList(ref result, t);
                return;
            }

            if (result.length == 2)
            {
                Vector3 forward = (result.array[1] - result.array[0]).normalized;
                result.array[0] = Vector3.Lerp(result.array[0], result.array[1], t);
                result.array[1] = forward;
            }
        }

        private static float ClampT(float t, LoopType type)
        {
            switch (type)
            {
                case LoopType.Once:
                    t = Mathf.Clamp01(t);
                    break;
                case LoopType.Loop:
                    t = Mathf.Repeat(t, 1f);
                    break;
                case LoopType.PingPong:
                    t = Mathf.PingPong(t, 1f);
                    break;
            }
            return t;
        }
    }
}
