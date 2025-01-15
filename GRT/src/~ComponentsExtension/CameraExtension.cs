namespace GRT
{
    using UnityEngine;

    public static class CameraExtension
    {
        public static bool Raycast(this Camera camera, out RaycastHit hit, float distance = -1f, int layerMask = -1)
        {
            if (camera != null)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                float d = distance > 0f ? distance : camera.farClipPlane;
                int m = layerMask >= 0 ? layerMask : camera.cullingMask;
                if (Physics.Raycast(ray, out hit, d, m))
                {
                    return true;
                }
            }
            hit = new RaycastHit();
            return false;
        }
    }
}