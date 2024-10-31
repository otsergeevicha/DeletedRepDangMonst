using Enemies;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Ammo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : MonoCache
    {
        private const string LayerNameEnemy = "Enemy";

        private BulletData _bulletData;
        private Collider[] _hits = new Collider[15];
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        private bool _isActive;
        private float _distanceBefore;
        private float _speed;
        private EffectModule _effectModule;
        private int _layerMask;

        public void Construct(BulletData bulletData, EffectModule effectModule)
        {
            _effectModule = effectModule;
            _bulletData = bulletData;
            _speed = _bulletData.MissileSpeed;

            _layerMask = 1 << LayerMask.NameToLayer(LayerNameEnemy);
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
            if (collision.gameObject.TryGetComponent(out Enemy invasionEnemy))
            {
                Physics.OverlapSphereNonAlloc(invasionEnemy.transform.position, _bulletData.RadiusExplosion, _hits, _layerMask);

                for (int i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i] != null && _hits[i].gameObject.TryGetComponent(out Enemy enemy))
                    {
                        if (!enemy.IsDie) 
                            enemy.ApplyDamage(_bulletData.MissileDamage);
                    }

                    _hits[i] = null;
                }

                _effectModule.OnExplosion(transform.position);
                InActive();
            }
        }

        public void InActive()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        public void Throw(Vector3 currentPosition, Vector3 targetPosition)
        {
            transform.position = currentPosition;

            _targetPosition = targetPosition;
            _distanceBefore = Vector3.Distance(currentPosition, _targetPosition);
            _moveDirection = (_targetPosition - currentPosition).normalized;

            gameObject.SetActive(true);

            _isActive = true;
        }
    }
}