using System;
using UnityEngine;
using UnityEngine.Events;

namespace GRT.Events
{
    [Serializable]
    public class Event3D : UnityEvent<Camera, RaycastHit, Vector2> { }
}