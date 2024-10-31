using System;
using System.Collections.Generic;
using System.Linq;
using Ammo;
using CameraModule;
using Canvases;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.ShootingModule
{
    public class WeaponHolder : MonoCache
    {
        [SerializeField] private Weapon[] _weapons;
        [SerializeField] private AudioSource _audioSource;

        private Hud _hud;

        public event Action OnChanged;
        
        public void Construct(CameraFollow cameraFollow,
            BulletData bulletData, EffectModule effectModule, Hud hud, IReadOnlyList<Bullet> poolBullets)
        {
            _hud = hud;
            
            foreach (Weapon weapon in _weapons)
                weapon.Construct(cameraFollow, _audioSource, hud, poolBullets);

            _hud.WeaponButtons.OnChanged += ChangeGun;
            
            Disarmed(true);
        }

        protected override void OnDisabled() => 
            _hud.WeaponButtons.OnChanged -= ChangeGun;

        public Weapon GetActiveGun() => 
            _weapons.FirstOrDefault(gun => 
                gun.isActiveAndEnabled);

        public void Disarmed(bool heroOnBase)
        {
            if (heroOnBase == false) 
                _weapons[0].gameObject.SetActive(true);

            if(heroOnBase)    
            {
                foreach (Weapon weapon in _weapons) 
                    weapon.gameObject.SetActive(false);
            }
        }

        private void ChangeGun(int currentGun)
        {
            foreach (Weapon weapon in _weapons)
            {
                weapon.gameObject.SetActive(false);
                weapon.OffShoot();
            }

            _weapons[currentGun].gameObject.SetActive(true);

            OnChanged?.Invoke();
        }

        public void UpdateLevel()
        {
            foreach (Weapon weapon in _weapons)
                weapon.UpdateLevel();
        }
    }
}