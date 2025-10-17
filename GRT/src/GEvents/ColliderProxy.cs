using UnityEngine;

namespace GRT.GEvents
{
    [RequireComponent(typeof(Collider))]
    public class ColliderProxy : MonoBehaviour
    {
        [SerializeField] private GameObject _realGameObject;

        private Collider _collider;

        public GameObject RealGameObject => _realGameObject == null ? _collider.gameObject : _realGameObject;
        public Collider Collider => _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
    }

    public static class ColliderProxyExtensions
    {
#if COLLIDER_PROXY
        private static ColliderProxy _cache;
#endif

        public static GameObject GetRealGameObject(this Collider collider)
        {
#if COLLIDER_PROXY
            if (_cache != null && _cache.Collider == collider)
            {
                return _cache.RealGameObject;
            }
            else
            {
                if (collider.gameObject.TryGetComponent<ColliderProxy>(out _cache))
                {
                    return _cache.RealGameObject;
                }
                else
                {
                    return collider.gameObject;
                }
            }
#else
            return collider.gameObject;
#endif
        }
    }
}