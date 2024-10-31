using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace Markers
{
    public class EnemyRing : MonoCache
    {
        private Transform _followingTransform;
        private Enemy _enemy;

        private void Start() =>
            InActive();

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            if (_enemy.IsDie)
            {
                InActive();
                return;
            }

            transform.position = _followingTransform.position;
        }

        public void OnActive(Enemy enemy)
        {
            _enemy = enemy;
            gameObject.SetActive(true);
            _followingTransform = enemy.transform;
        }

        public void InActive()
        {
            gameObject.SetActive(false);
            _followingTransform = null;
        }
    }
}