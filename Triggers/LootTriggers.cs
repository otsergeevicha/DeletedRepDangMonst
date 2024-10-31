using System;
using ContactZones;
using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class LootTriggers : MonoCache
    {
        public Action<StorageGem> StorageGemEntered;
        public Action<StorageGem> StorageGemExited;
        
        public event Action<int> OnPickUpMoney;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Money money))
            {
                OnPickUpMoney?.Invoke(money.CurrentNominal);
                money.PickUp();
            }

            if (collision.gameObject.TryGetComponent(out StorageGem storageGem)) 
                StorageGemEntered?.Invoke(storageGem);
        }
        
        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out StorageGem storageGem)) 
                StorageGemExited?.Invoke(storageGem);
        }
    }
}