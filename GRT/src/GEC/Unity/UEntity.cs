using System;
using System.Collections.Generic;
using UnityEngine;

namespace GRT.GEC.Unity
{
    public class UEntity : IGEntity<GameObject>, ILender<UEntity>
    {
        public WeakReference<GameObject> Reference { get; private set; }

        public string Location { get; private set; }

        public IList<IGComponent<GameObject>> Components { get; } = new List<IGComponent<GameObject>>();

        public UEntity Wares => this;

        public ICollection<IBorrower<UEntity>> Borrowers { get; } = new List<IBorrower<UEntity>>();

        public void Dun()
        {
            foreach (var borrower in Borrowers)
            {
                Notary<UEntity>.Return(borrower, this, false);
            }

            Borrowers.Clear();
        }

        public void Load()
        {
            GameObject go = Location.CanBeSplitBy(':', out var scene, out var path)
                ? GameObjectExtension.FindIn(scene, path)
                : GameObjectExtension.FindIn(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), path);

            Reference = new WeakReference<GameObject>(go);

            foreach (var com in Components)
            {
                if (com is IBorrower<UEntity> borrower)
                {
                    Notary<UEntity>.Borrow(borrower, this); // 为毛是借出人主动发起???
                }
            }
        }
    }
}