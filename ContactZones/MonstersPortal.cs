using System;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class MonstersPortal : MonoCache
    {
        public event Action OnEscaped;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Escape();
                OnEscaped?.Invoke();
            }
        }
    }
}