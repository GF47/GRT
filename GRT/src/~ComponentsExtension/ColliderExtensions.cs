using UnityEngine;

namespace GRT
{
    public static class ColliderExtensions
    {
        public static void ResizeToWrapChildren(this BoxCollider box)
        {
            var max = Vector3.negativeInfinity;
            var min = Vector3.positiveInfinity;

            var renderers = box.gameObject.GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0)
            {
                throw new UnityException($"{box.gameObject.GetPath(true)} has no renderer, can not be wrapped by a box collider");
            }

            for (int i = 0; i < renderers.Length; i++)
            {
                // bounds.Encapsulate(renderers[i].bounds);
                max = Vector3.Max(max, renderers[i].bounds.max);
                min = Vector3.Min(min, renderers[i].bounds.min);
            }

            var matrix = box.transform.worldToLocalMatrix;

            box.center = matrix.MultiplyPoint((max + min) / 2f);
            var size = matrix.MultiplyVector(max - min);
            box.size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
        }
    }
}