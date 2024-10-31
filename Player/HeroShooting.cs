using System;
using System.Collections.Generic;
using Enemies;
using Markers;
using Modules;
using Player.ShootingModule;
using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class HeroShooting : MonoCache
    {
        [SerializeField] private Animator _animator;

        private readonly float _waitingDownGun = 2f;

        private WeaponHolder _weaponHolder;
        private HeroMovement _heroMovement;

        private bool _heroOnBase = true;
        private float _heroDataRadiusDetection;
        private List<Enemy> _enemies;
        private EnemyRing _enemyRing;
        private bool _haveTarget;
        private Enemy _currentEnemy;
        private int _requestTarget;

        private float _timerDownGun;

        public void Construct(HeroMovement heroMovement,
            WeaponHolder weaponHolder, float heroDataRadiusDetection, EnemyRing enemyRing)
        {
            _enemyRing = enemyRing;
            _heroDataRadiusDetection = heroDataRadiusDetection;

            _heroMovement = heroMovement;
            _weaponHolder = weaponHolder;

            _weaponHolder.OnChanged += () =>
            {
                _currentEnemy = null;
                _requestTarget = 0;
            };
        }

        protected override void UpdateCached()
        {
            if (_heroOnBase)
                return;

            FindNearestEnemy();

            if (_requestTarget == 1)
            {
                _timerDownGun = _waitingDownGun;

                _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, 1f));
                _heroMovement.SetStateBattle(true, _currentEnemy.transform);
                _enemyRing.OnActive(_currentEnemy);

                Shoot(_currentEnemy);
            }

            if (!_haveTarget)
            {
                _timerDownGun -= Time.deltaTime;

                if (_timerDownGun <= Single.Epsilon)
                    _animator.SetLayerWeight(1, 0);

                _heroMovement.SetStateBattle(false, null);
                _requestTarget = 0;
                OffShoot();
                _enemyRing.InActive();
            }
        }

        protected override void OnDisabled()
        {
            if (_currentEnemy != null)
                _currentEnemy.Died -= UpdateTarget;
        }

        public void SetOnBase(bool heroOnBase)
        {
            _heroOnBase = heroOnBase;
            _animator.SetLayerWeight(1, 0);
            _requestTarget = 0;
            OffShoot();
            _enemyRing.InActive();
            _haveTarget = false;
            _weaponHolder.Disarmed(heroOnBase);
        }

        public void MergeEnemies(List<Enemy> poolSimpleEnemies, List<Enemy> poolBosses)
        {
            _enemies = new List<Enemy>();

            _enemies.AddRange(poolSimpleEnemies);
            _enemies.AddRange(poolBosses);

            OffShoot();
            UpdateTarget();
            _haveTarget = false;
        }

        public void Upgrade(float newRadiusDetection) =>
            _heroDataRadiusDetection = newRadiusDetection;

        private void FindNearestEnemy()
        {
            float minDistance = _heroDataRadiusDetection;
            Vector3 heroPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            foreach (Enemy enemy in _enemies)
            {
                float distance = Vector3.Distance(heroPosition, enemy.transform.position);

                if (distance <= _heroDataRadiusDetection && distance < minDistance && enemy.IsDie == false)
                {
                    if (_currentEnemy != null)
                        _currentEnemy.Died -= UpdateTarget;

                    _currentEnemy = enemy;
                    _requestTarget++;

                    if (_currentEnemy != enemy) 
                        _requestTarget = 1;

                    _haveTarget = true;
                    _currentEnemy.Died += UpdateTarget;

                    return;
                }

                UpdateTarget();
                _haveTarget = false;
            }
        }

        private void UpdateTarget()
        {
            _currentEnemy = null;
            _requestTarget = 0;
            _enemyRing.InActive();
        }

        private void Shoot(Enemy enemy)
        {
            if (!_heroOnBase)
                _weaponHolder.GetActiveGun().Shoot(enemy);
        }

        private void OffShoot()
        {
            if (_currentEnemy != null)
                _currentEnemy.Died -= UpdateTarget;

            _weaponHolder.GetActiveGun()?.OffShoot();
        }
    }
}