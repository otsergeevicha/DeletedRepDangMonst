using System;
using ContactZones;
using Plugins.MonoCache;
using Turrets.Children;
using UnityEngine;

namespace Triggers
{
    public class AmmoTriggers : MonoCache
    {
        public Action StorageEntered;
        public Action StorageExited;
        
        public Action<CartridgeGun> CartridgeGunEntered;
        public Action<CartridgeGun> CartridgeGunExited;
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out StorageAmmoPlate _)) 
                StorageEntered?.Invoke();
            
            if (collision.TryGetComponent(out CartridgeGun cartridge)) 
                CartridgeGunEntered?.Invoke(cartridge);
        }
        
        private void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out StorageAmmoPlate _)) 
                StorageEntered?.Invoke();
            
            if (collision.TryGetComponent(out CartridgeGun cartridge)) 
                CartridgeGunEntered?.Invoke(cartridge);
        }
        
        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out StorageAmmoPlate _)) 
                StorageExited?.Invoke();
            
            if (collision.TryGetComponent(out CartridgeGun cartridge)) 
                CartridgeGunExited?.Invoke(cartridge);
        }
    }
}