using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ammo;
using CameraModule;
using Canvases;
using Cysharp.Threading.Tasks;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace Player.ShootingModule
{
    public class Weapon : MonoCache
    {
        [Range(1, 100)] 
        [SerializeField] private int _countBullet;

        [SerializeField] private int _indexWeapon;
        [SerializeField] private Transform _spawnPointBullet;

        [Range(0.1f, 1f)] 
        [SerializeField] private float _shotDelay;

        [Range(1, 50)] 
        [SerializeField] private int _damageCorrection;

        private IMagazine _magazine;
        private Enemy _currentTarget;
        private bool _isAttack;
        private CameraFollow _camera;
        private AudioSource _audioSource;
        private IReadOnlyList<Bullet> _poolBullets;

        private float _lastShotTime;

        public void Construct(CameraFollow cameraFollow,
            AudioSource audioSource, Hud hud,
            IReadOnlyList<Bullet> poolBullets)
        {
            _poolBullets = poolBullets;
            _audioSource = audioSource;
            _camera = cameraFollow;
            _magazine = new MagazineBullets(_countBullet, hud, _indexWeapon);
        }

        protected override void UpdateCached()
        {
            if (!_isAttack || !isActiveAndEnabled)
            {
                if (_currentTarget != null)
                    _currentTarget.Died -= OffShoot;
                
                return;
            }

            if (!(Time.time >= _lastShotTime + _shotDelay))
                return;

            if (_magazine.Check())
            {
                _audioSource.Play();

                _poolBullets.FirstOrDefault(bullet => !bullet.isActiveAndEnabled)?.Shoot(
                    _spawnPointBullet.position,
                    new Vector3(_currentTarget.transform.position.x, 1f, _currentTarget.transform.position.z),
                    _damageCorrection);
                
               // _camera.Shake();

                _magazine.Spend();
                _lastShotTime = Time.time;
            }

            _magazine.Shortage();
        }

        protected override void OnDisabled()
        {
            if (_currentTarget != null)
                _currentTarget.Died -= OffShoot;
        }

        public void Shoot(Enemy enemy)
        {
            if (_isAttack)
                return;

            _isAttack = true;
            _currentTarget = enemy;
            _currentTarget.Died += OffShoot;

            _lastShotTime = Time.time;
        }

        public void OffShoot()
        {
            _isAttack = false;

            if (_currentTarget != null)
                _currentTarget.Died -= OffShoot;

            _currentTarget = null;
        }

        public void UpdateLevel() =>
            _magazine.UpdateLevel();
    }
}