using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.Animation
{
    public class HeroAnimation : MonoCache
    {
        [SerializeField] private Animator _animator;

        private HeroData _heroData;
        private int _runHash;
        private bool _heroOnBase = true;

        public void Construct(HeroData heroData)
        {
            _heroData = heroData;
            _runHash = _heroData.RunHash;
        }

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void SetActualRunHash(bool heroOnBase)
        {
            _heroOnBase = heroOnBase;
            
            _animator.SetBool(_runHash, false);
            _runHash = heroOnBase ? _heroData.RunHash : _heroData.RunGunHash;
        }

        public void EnableRun()
        {
            _animator.SetBool(_heroData.IdleAimingHash, false);
            _animator.SetBool(_runHash, true);
        }

        public void EnableIdle()
        {
            if (!_heroOnBase) 
                _animator.SetBool(_heroData.IdleAimingHash, true);
            
            if (_heroOnBase) 
                _animator.SetBool(_runHash, false);
        }
    }
}