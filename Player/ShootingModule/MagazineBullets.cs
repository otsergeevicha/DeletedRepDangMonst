using System;
using Canvases;

namespace Player.ShootingModule
{
    public class MagazineBullets : IMagazine
    {
        private const int DelayRegenerationMagazine = 500;

        private readonly MagazineReload _magazineReload;
        private readonly int _maxSize;
        private readonly Hud _hud;
        private readonly int _indexWeapon;
        private int _size;

        public MagazineBullets(int maxCountBullets, Hud hud, int indexWeapon)
        {
            _indexWeapon = indexWeapon;
            _hud = hud;
            hud.WeaponReload(false);
            _size = maxCountBullets;
            _maxSize = maxCountBullets;
            _magazineReload = new MagazineReload(this);
            _hud.WeaponButtons.UpdateViewBullets(_indexWeapon, _size, _maxSize);
        }

        public void Spend()
        {
            _size--;
            _hud.WeaponButtons.UpdateViewBullets(_indexWeapon, _size, _maxSize);
        }

        public bool Check()
        {
            if (_size == 0)
            {
                _hud.WeaponReload(true);
                return false;
            }

            _hud.WeaponReload(false);
            return true;
        }

        public void Replenishment(Action fulled)
        {
            _size++;

            if (_size >= _maxSize)
            {
                _magazineReload.StopReplenishment();
                fulled?.Invoke();
                _hud.WeaponButtons.UpdateViewBullets(_indexWeapon, _size, _maxSize);
                return;
            }
            
            _hud.WeaponButtons.UpdateViewBullets(_indexWeapon, _size, _maxSize);
        }

        public void Shortage()
        {
            if (_magazineReload.IsCharge)
                return;

            if (_size == 0)
            {
                _magazineReload.StopReplenishment();
                _magazineReload.Launch(DelayRegenerationMagazine).Forget();
            }
        }

        public void UpdateLevel()
        {
            _size = _maxSize;
            _hud.WeaponButtons.UpdateViewBullets(_indexWeapon, _size, _maxSize);
        }
    }
}