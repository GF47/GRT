using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    [Serializable]
    public class GnityEvent : UnityEvent<Camera, RaycastHit, Vector2> { }
}