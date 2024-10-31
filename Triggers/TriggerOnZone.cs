using System;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class TriggerOnZone : MonoCache
    {
        public event Action OnEntered;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _))
                OnEntered?.Invoke();
        }
    }
}