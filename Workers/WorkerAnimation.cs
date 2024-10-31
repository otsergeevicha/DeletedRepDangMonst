using System;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Workers
{
    [RequireComponent(typeof(Animator))]
    public class WorkerAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private WorkerData _workerData;

        public void Construct(WorkerData workerData) => 
            _workerData = workerData;

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void EnableSitingIdle() => 
            _animator.SetBool(_workerData.IdleSitingHash, true);

        public void EnableRun()
        {
            _animator.SetBool(_workerData.IdleSitingHash, false);
            _animator.SetBool(_workerData.WalkingHash, false);
            _animator.SetBool(_workerData.MiningHash, false);
            
            _animator.SetBool(_workerData.SlowRunHash, true);
        }

        public void EnableWalk()
        {
            _animator.SetBool(_workerData.IdleSitingHash, false);
            _animator.SetBool(_workerData.SlowRunHash, false);
            _animator.SetBool(_workerData.MiningHash, false);
            
            _animator.SetBool(_workerData.WalkingHash, true);
        }

        public void EnableIdle()
        {
            _animator.SetBool(_workerData.IdleSitingHash, false);
           _animator.SetBool(_workerData.SlowRunHash, false);
           _animator.SetBool(_workerData.WalkingHash, false);
           _animator.SetBool(_workerData.MiningHash, false);
        }

        public void EnableMine()
        {
            _animator.SetBool(_workerData.IdleSitingHash, false);
            _animator.SetBool(_workerData.SlowRunHash, false);
            _animator.SetBool(_workerData.WalkingHash, false);
            
            _animator.SetBool(_workerData.MiningHash, true);
        }
    }
}