using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject, UEntity>
    {
        private readonly ICollection<IUser<GameObject>> _user = new List<IUser<GameObject>>();

        public GameObject Ware { get; private set; }

        public string Location { get; set; }

        public IList<IGComponent<GameObject, UEntity>> Components { get; } = new List<IGComponent<GameObject, UEntity>>();

        public void Provide(IUser<GameObject> user) => _user.Add(user);

        public virtual async Task LoadWare(GameObject ware = null) => Ware = ware == null ? GameObjectExtension.FindByLocation(Location) : ware;

        public void CancelProvide()
        {
            foreach (var user in _user)
            {
                Notary<GameObject>.Cancel(this, user);
            }

            _user.Clear();
        }
    }
}