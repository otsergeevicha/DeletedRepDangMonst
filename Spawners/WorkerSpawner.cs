using System;
using Canvases;
using Infrastructure.Factory.Pools;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;
using Workers;

namespace Spawners
{
    public class WorkerSpawner : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private WorkerSpawnPoint[] _workerSpawnPoints = new WorkerSpawnPoint[4];
        
        private PoolWorkers _pool;

        public event Action OnTutorialContacted;
        
        public void Construct(PoolWorkers pool, Vector3 workplace, PoolData poolData)
        {
            _pool = pool;
            
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints) 
                spawnPoint.Construct(pool, workplace, this, poolData);
        }

        public void UpdateLevel()
        {
            foreach (Worker worker in _pool.Workers)
            {
                worker.InActive();
                worker.Reset();
            }
            
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints)
                spawnPoint.OnActiveSpawner();
        }

        public void Notify() => 
            OnTutorialContacted?.Invoke();

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;

        public Transform GetRootCamera() => 
            _rootCamera;
    }
}