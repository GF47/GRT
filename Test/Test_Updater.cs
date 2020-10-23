using UnityEngine;
using GRT.Updater;
using System;

namespace Test
{
    public class Test_Updater : MonoBehaviour
    {
        Vector3 f = new Vector3(1, 1, 1);
        [SerializeField]
        Vector3 t = new Vector3(0, -2, 5);

        Vector3Buffer buffer;

        void Start()
        {
            buffer = new Vector3Buffer(f, v => transform.localScale = v, 2f);
        }

        void Update()
        {
            if ((buffer.To-t).magnitude > 1e-6f)
            {
                buffer.To = t;
            }
        }
    }
}
