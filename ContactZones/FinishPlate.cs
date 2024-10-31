using System;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class FinishPlate : MonoCache
    {
        public event Action Finished;
        
        private float _waitTime = 2f;
        private bool _isWaiting;

        public void OnActive() => 
            gameObject.SetActive(true);

        public void InActive() => 
            gameObject.SetActive(false);
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.TryGetComponent(out Hero _))
                _isWaiting = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out Hero _)) 
                _isWaiting = false;
        }
        
        protected override void UpdateCached()
        {
            if (_isWaiting)
            {
                _waitTime -= Time.deltaTime;

                if (_waitTime <= Single.Epsilon)
                {
                    _isWaiting = false;
                    _waitTime = 2f;
                    Finished?.Invoke();
                }
            }
        }
    }
}