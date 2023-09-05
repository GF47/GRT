using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject, UEntity>, IProvider<UEntity>
    {
        private readonly ICollection<IUser<UEntity>> _user = new List<IUser<UEntity>>();

        public UEntity Ware => this;

        public GameObject Puppet { get; private set; }

        public string Location { get; set; }

        public IList<IGComponent<GameObject, UEntity>> Components { get; } = new List<IGComponent<GameObject, UEntity>>();

        public void Provide(IUser<UEntity> user) => _user.Add(user);

        public virtual async Task SetPuppet(GameObject puppet = null) => Puppet = puppet == null ? GameObjectExtension.FindByLocation(Location) : puppet;

        public void CancelProvide()
        {
            foreach (var user in _user)
            {
                Notary<UEntity>.Cancel(this, user);
            }

            _user.Clear();
        }
    }
}