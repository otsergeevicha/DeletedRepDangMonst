using System;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    [RequireComponent(typeof(SphereCollider))]
    public class TurretTrigger : MonoCache
    {
        [HideInInspector] [SerializeField] private SphereCollider _collider;
        
        public event Action Invasion;

        private void OnValidate() => 
            _collider ??= Get<SphereCollider>();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.TryGetComponent(out Enemy _)) 
                Invasion?.Invoke();
        }

        public void OnActiveCollider() => 
            _collider.enabled = true;

        public void InActiveCollider() => 
            _collider.enabled = false;

        public void SetRadiusTrigger(float newRadius) =>
            _collider.radius = newRadius;
    }
}