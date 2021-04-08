namespace GRT
{
    using UnityEngine;

    public static class CameraExtension
    {
        public static bool Raycast(this Camera camera, out RaycastHit hit, float distance = -1f)
        {
            if (camera != null)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                float d = distance > 0f ? distance : camera.farClipPlane;
                if (Physics.Raycast(ray, out hit, d, camera.cullingMask))
                {
                    return true;
                }
            }
            hit = new RaycastHit();
            return false;
        }
    }
}