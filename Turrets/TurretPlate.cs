using Plugins.MonoCache;
using Turrets.Children;
using UnityEngine;

namespace Turrets
{
    public class TurretPlate : MonoCache
    {
        [SerializeField] private CartridgeGun _cartridge;

        public CartridgeGun GetCartridgeGun => 
            _cartridge;
    }
}