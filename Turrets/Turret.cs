using System.Collections;
using System.Linq;
using Ammo;
using Canvases;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;

namespace Turrets
{
    public class Turret : MonoCache
    {
        [SerializeField] private TurretTrigger _trigger;
        [SerializeField] private Transform _spawnPointGrenade;

        [SerializeField] private CanvasTurretLowAmmo _canvasTurret;
        [SerializeField] private Animation _animationLowAmmo;

        private const string LayerNameEnemy = "Enemy";

        private readonly Collider[] _hits = new Collider[1];

        private TurretData _turretData;
        private PoolMissiles _poolMissiles;
        private CartridgeGun _cartridgeGun;
        private Transform _turretBody;
        private bool _isAttack;
        private float _lastShotTime;
        private float _shotDelay;

        private int _layerMask;

        private int _price;
        private Vector3 _currentTarget;
        private Vector3 _fromTo;
        private Quaternion _lookRotation;

        public void Construct(CartridgeGun cartridgeGun, TurretData turretData, PoolMissiles poolMissiles,
            BaseGate baseGate)
        {
            _cartridgeGun = cartridgeGun;
            _poolMissiles = poolMissiles;
            _turretData = turretData;
            _turretBody = transform;
            _shotDelay = turretData.ShotDelay;

            _canvasTurret.transform.SetParent(null);
            _layerMask = 1 << LayerMask.NameToLayer(LayerNameEnemy);

            baseGate.OnHit += () =>
            {
                if (isActiveAndEnabled)
                    CheckEnemy();
            };
        }

        protected override void UpdateCached()
        {
            if (!_isAttack || !isActiveAndEnabled)
                return;

            if (!(Time.time >= _lastShotTime + _shotDelay))
                return;

            _turretBody.rotation = Quaternion.RotateTowards(_turretBody.rotation, _lookRotation,
                _turretData.RotateSpeed * Time.deltaTime);

            if (_turretBody.rotation == _lookRotation)
                Shoot(_currentTarget);
        }

        public void OnActive(Transform spawnPoint, int currentPrice)
        {
            _price = currentPrice;
            gameObject.SetActive(true);
            transform.position = spawnPoint.position;

            _trigger.OnActiveCollider();
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
            _trigger.Invasion += CheckEnemy;
            _cartridgeGun.OnCharge += CheckEnemy;
        }

        public void InActive()
        {
            _animationLowAmmo.Stop();
            _trigger.InActiveCollider();
            _trigger.Invasion -= CheckEnemy;
            _cartridgeGun.OnCharge -= CheckEnemy;
            gameObject.SetActive(false);
        }

        public void Upgrade()
        {
            _turretData.RadiusDetection++;
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
        }

        public int GetPrice() =>
            _price;

        public void IncreasePrice(int stepIncreasePrice) =>
            _price += stepIncreasePrice;
        
        private void CheckEnemy()
        {
            if (DetectEnemy(_turretData.RadiusDetection)
                || DetectEnemy(_turretData.RadiusDetection / 2)
                || DetectEnemy(_turretData.RadiusDetection / 3))
            {
                PreFire();
            }
            else
            {
                _isAttack = false;
            }
        }

        private bool DetectEnemy(float radius)
        {
            Overlap(radius);
            return _hits[0] != null;
        }

        private void PreFire()
        {
            if (_hits[0].gameObject.TryGetComponent(out Enemy enemy))
            {
                _isAttack = true;

                enemy.Died += () =>
                {
                    _isAttack = false;
                    CheckEnemy();
                };

                _currentTarget = enemy.transform.position;

                _fromTo = _currentTarget - transform.position;
                _fromTo.y = .0f;
                _lookRotation = Quaternion.LookRotation(_fromTo);

                _hits[0] = null;
            }
            else
            {
                _isAttack = false;
            }
        }

        private void Overlap(float cleavage) => 
            Physics.OverlapSphereNonAlloc(transform.position, cleavage, _hits, _layerMask);

        private void Shoot(Vector3 targetPosition)
        {
            Missile missile = _poolMissiles.Missiles.FirstOrDefault(bullet =>
                bullet.isActiveAndEnabled == false);

            if (missile != null && _cartridgeGun.CheckMagazine())
            {
                missile.Throw(_spawnPointGrenade.position, new Vector3(targetPosition.x, 1f, targetPosition.z));
                _cartridgeGun.Spend();
                _lastShotTime = Time.time;
                CheckEnemy();
            }
        }
    }
}