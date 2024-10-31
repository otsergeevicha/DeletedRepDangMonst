using Enemies;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Ammo
{
    public class Bullet : MonoCache
    {
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        private bool _isActive;
        private float _distanceBefore;
        private int _currentDamage;
        private float _speed;
        private EffectModule _effectModule;

        public void Construct(BulletData bulletData, EffectModule effectModule)
        {
            _effectModule = effectModule;
            _speed = bulletData.MissileSpeed;
            _currentDamage = bulletData.BulletDamage;
        }

        protected override void FixedUpdateCached()
        {
            if (!_isActive)
                return;
            
            transform.position += _moveDirection * (Time.fixedDeltaTime * _speed);
            
            if (_distanceBefore <= Vector3.Distance(transform.position, _targetPosition)) 
                InActive();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_currentDamage);
                _effectModule.OnHitEnemy(enemy.gameObject.transform.position);
            }
        }

        public void Shoot(Vector3 currentPosition, Vector3 targetPosition, int damageCorrection)
        {
            _currentDamage += damageCorrection;
            
            transform.position = currentPosition;
            
            _targetPosition = targetPosition;
            _distanceBefore = Vector3.Distance(currentPosition, _targetPosition);
            _moveDirection = (_targetPosition - currentPosition).normalized;
            
            gameObject.SetActive(true);
            
            _isActive = true;
        }

        public void InActive()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}