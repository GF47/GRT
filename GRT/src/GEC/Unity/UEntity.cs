using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject, UEntity>, IProvider<UEntity>
    {
        private readonly ICollection<IConsumer<UEntity>> _consumer = new List<IConsumer<UEntity>>();

        public UEntity Ware => this;

        public GameObject Puppet { get; internal set; }

        public string Location { get; set; }

        public IList<IGComponent<GameObject, UEntity>> Components { get; } = new List<IGComponent<GameObject, UEntity>>();

        public void Provide(IConsumer<UEntity> consumer) => _consumer.Add(consumer);

        public void CancelProvide()
        {
            foreach (var consumer in _consumer)
            {
                Contract<UEntity>.Cancel(this, consumer);
            }

            _consumer.Clear();
        }

        // public virtual async Task SetPuppet(GameObject puppet = null) => Puppet = puppet == null ? GameObjectExtension.FindByLocation(Location) : puppet;
    }
}