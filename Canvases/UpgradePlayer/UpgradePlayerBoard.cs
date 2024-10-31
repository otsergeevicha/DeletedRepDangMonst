using System;
using Modules;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases.UpgradePlayer
{
    public class UpgradePlayerBoard : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;

        public event Action OnEntered;
        public event Action OnTutorialContacted;
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _))
            {
                OnEntered?.Invoke();
                OnTutorialContacted?.Invoke();
            }
        }

        public Transform GetRootCamera() => 
            _rootCamera;

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;
    }
}