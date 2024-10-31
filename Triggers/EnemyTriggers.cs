using System;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class EnemyTriggers : MonoCache
    {
        [SerializeField] private SphereCollider _collider;

        public event Action<BaseGate> OnAgroGate;
        public event Action NonAgroGate;
        public event Action OnAgro;
        public event Action NonAgro;

        private void OnValidate() => 
            _collider ??= Get<SphereCollider>();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out BaseGate gate))
            {
                OnAgroGate?.Invoke(gate);
                return;
            }

            if (collision.gameObject.TryGetComponent(out Hero _)) 
                OnAgro?.Invoke();
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out BaseGate _))
            {
                NonAgroGate?.Invoke();
                return;
            }
            
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                NonAgro?.Invoke();
        }

        public void SetRadius(int newAgroDistance) => 
            _collider.radius = newAgroDistance;
    }
}