using System;
using System.Linq;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using SO;
using Spawners;
using UnityEngine;
using UnityEngine.UI;
using Workers;

namespace Canvases
{
    public class WorkerSpawnPoint : MonoCache
    {
        [SerializeField] private Image _image;
        [SerializeField] private BoxCollider _collider;

        private readonly float _timerSeconds = 30f;
        private readonly float _waitTime = 3f;

        private float _timerWaiting;
        private bool _isWaiting;
        private bool _isEmptyPlace;
        private bool _endSpawn;
        private float _currentFillAmount = 1f;
        private Worker _currentWorker;
        private PoolWorkers _workers;
        private Vector3 _workplace;
        private WorkerSpawner _workerSpawner;
        private PoolData _poolData;

        public void Construct(PoolWorkers workers, Vector3 workplace,
            WorkerSpawner workerSpawner, PoolData poolData)
        {
            _poolData = poolData;
            _workerSpawner = workerSpawner;
            _workplace = workplace;
            _workers = workers;
            _timerWaiting = _timerSeconds;
            
            Spawn();
        }

        private void OnTriggerEnter(Collider collision)
        {
            ResetFill();

            if (collision.TryGetComponent(out Hero _))
                _isWaiting = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            ResetFill();

            if (collision.TryGetComponent(out Hero _))
            {
                _isWaiting = false;
                _currentFillAmount = 1;
            }
        }

        protected override void UpdateCached()
        {
            if (!_endSpawn)
            {
                if (_isWaiting)
                {
                    _currentFillAmount -= Time.deltaTime / _waitTime;
                    _image.fillAmount = _currentFillAmount;

                    if (_currentFillAmount <= Single.Epsilon)
                    {
                        FinishWaiting();
                        _isWaiting = false;
                        _currentFillAmount = 1f;
                    }
                }

                if (_isEmptyPlace)
                {
                    if (_timerWaiting > 0)
                    {
                        _timerWaiting -= Time.deltaTime;
                    }
                    else
                    {
                        Interactable(true);
                        _image.fillAmount = _currentFillAmount;
                        Spawn();
                        _timerWaiting = _timerSeconds;
                        _isEmptyPlace = false;
                    }
                }
            }
        }

        public void ResetLogic() => 
            _endSpawn = false;

        public void OnActiveSpawner()
        {
            _endSpawn = false;
            Spawn();
        }

        private void Spawn()
        {
            _currentWorker = _workers.Workers.FirstOrDefault(worker => worker.isActiveAndEnabled == false);

            if (_currentWorker != null)
            {
                _currentWorker.OnActive();
                _currentWorker.SetMannequin(false);
                _currentWorker.transform.position = new Vector3(transform.position.x, -.37f, transform.position.z);
                _currentWorker.transform.rotation = Quaternion.Euler(-42f, 180f, 0f);
                _isEmptyPlace = false;
            }
            else
            {
                _endSpawn = true;
                Interactable(false);
            }
        }

        private void FinishWaiting()
        {
            _currentWorker.SetMannequin(true);
            _currentWorker.SendWorkplace(_workplace);
            Interactable(false);
            _isEmptyPlace = true;
            _workerSpawner.Notify();
        }

        private void Interactable(bool flag)
        {
            _image.enabled = flag;
            _collider.enabled = flag;
        }

        private void ResetFill() =>
            _image.fillAmount = 1;
    }
}