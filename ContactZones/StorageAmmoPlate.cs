using System;
using Modules;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class StorageAmmoPlate : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private Transform _markerPosition;
        public event Action OnTutorialContacted;
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                OnTutorialContacted?.Invoke();
        }

        public Transform GetRootCamera() => 
            _rootCamera;

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;
    }
}