using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Loots;
using Modules;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class StorageGem : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private Gem[] _gems = new Gem[27];

        private const int MillisecondsDelay = 500;
        private const int CountReturnGems = 1;

        private bool _isReplenishment;
        
        private int _currentAmountGems = 0;
        private int _maxAmount;
        
        public event Action OnTutorialContacted;
        
        public bool IsFulled { get; private set; }

        public bool IsEmpty => 
            _currentAmountGems == 0;

        private void Start()
        {
            _maxAmount = _gems.Length;

            if (_maxAmount != 0)
            {
                for (int i = 0; i < _maxAmount; i++)
                    _gems[i].InActive();
            }
        }

        public void ApplyGem()
        {
            if (_currentAmountGems >= _maxAmount)
            {
                _currentAmountGems = _maxAmount;
                IsFulled = true;
            }
            
            OnTutorialContacted?.Invoke();
            
            for (int i = 0; i < _maxAmount; i++)
            {
                if (_gems[i].isActiveAndEnabled == false)
                {
                    _gems[i].OnActive();
                    _currentAmountGems++;
                    return;
                }
            }
        }

        public void HeroOut() => 
            _isReplenishment = false;

        public Transform GetRootCamera() => 
            _rootCamera;

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;

        private void Spend()
        {
            Gem gem = _gems.LastOrDefault(gem => gem.isActiveAndEnabled);
            
            if (gem != null)
            {
                _currentAmountGems--;
                IsFulled = false;
                
                if (_currentAmountGems < 0) 
                    _currentAmountGems = 0;
                
                gem.InActive();
            }
        }

        public async UniTaskVoid GetGem(Action<int> countGem)
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                if (IsEmpty)
                {
                    _isReplenishment = false;
                }
                else
                {
                    countGem?.Invoke(CountReturnGems);
                    Spend();
                }

                await UniTask.Delay(MillisecondsDelay);
            }
        }
    }
}