using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject>
    {
        private readonly ICollection<IUser<GameObject>> _user = new List<IUser<GameObject>>();

        public GameObject Ware { get; private set; }

        public string Location { get; set; }

        public IList<IGComponent<GameObject>> Components { get; } = new List<IGComponent<GameObject>>();

        public void Provide(IUser<GameObject> user) => _user.Add(user);

        public void Provide()
        {
            Ware = GameObjectExtension.FindByLocation(Location);

            foreach (var com in Components)
            {
                if (com is IUser<GameObject> user)
                {
                    Notary<GameObject>.Notarize(this, user);
                }
            }
        }

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