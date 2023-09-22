using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject, UEntity>, IProvider<UEntity>
    {
        private readonly ICollection<IConsumer<UEntity>> _consumer = new List<IConsumer<UEntity>>();

        public UEntity Ware => this;

        public GameObject Puppet { get; private set; }

        public string Location { get; set; }

        public IList<IGComponent<GameObject, UEntity>> Components { get; } = new List<IGComponent<GameObject, UEntity>>();

        public void Provide(IConsumer<UEntity> consumer) => _consumer.Add(consumer);

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行

        public virtual async Task SetPuppet(GameObject puppet = null) => Puppet = puppet == null ? GameObjectExtension.FindByLocation(Location) : puppet;

#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行

        public void CancelProvide()
        {
            foreach (var consumer in _consumer)
            {
                Contract<UEntity>.Cancel(this, consumer);
            }

            _consumer.Clear();
        }
    }
}