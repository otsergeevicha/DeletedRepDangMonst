using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules;
using Plugins.MonoCache;
using Services.Basket;
using UnityEngine;

namespace Turrets.Children
{
    public class CartridgeGun : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;

        [SerializeField] private CartridgeBox[] _cartridgeBoxes;

        private const int MillisecondsDelay = 500;
        private bool _isCourierExited;
        private int _currentAmountBoxes;
        private int _multiplier = 2;
        private int _maxAmount;

        public event Action OnTutorialContacted;
        public event Action OnCharge;
        public event Action OnNotifyAssistant;

        public bool IsRequiredDownload =>
            _currentAmountBoxes >= 0 && _currentAmountBoxes < _maxAmount;

        private void Start()
        {
            _maxAmount = _cartridgeBoxes.Length;

            if (_maxAmount != 0)
            {
                for (int i = 0; i < _maxAmount; i++)
                    _cartridgeBoxes[i].InActive();
            }
        }

        public bool CheckMagazine()
        {
            if (_currentAmountBoxes > 0)
                return true;

            OnNotifyAssistant?.Invoke();
            return false;
        }

        public void SetPresenceCourier(bool status) =>
            _isCourierExited = status;

        public void ApplyBox(IBasket basket)
        {
            if (_maxAmount == 0)
                return;

            if (_currentAmountBoxes == _maxAmount)
            {
                OnNotifyAssistant?.Invoke();
                return;
            }

            Replenishment(basket).Forget();
        }

        public void Spend()
        {
            if (_multiplier == 0)
            {
                _cartridgeBoxes.LastOrDefault(box => box.isActiveAndEnabled)?.InActive();
                _currentAmountBoxes--;
                _multiplier = 2;
                OnNotifyAssistant?.Invoke();
                return;
            }

            _multiplier--;
        }

        public Transform GetRootCamera() =>
            _rootCamera;

        public Vector3 GetPositionMarker() =>
            _markerPosition.transform.position;

        public void UpdateLevel()
        {
            foreach (CartridgeBox box in _cartridgeBoxes)
                box.InActive();

            _currentAmountBoxes = 0;
        }

        public int ReadCurrentAmmo() => 
            _currentAmountBoxes;

        private async UniTaskVoid Replenishment(IBasket basket)
        {
            for (int i = 0; i < _maxAmount; i++)
            {
                if (_isCourierExited)
                    return;

                if (basket.IsEmpty)
                    return;

                OnTutorialContacted?.Invoke();

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    _currentAmountBoxes++;
                    basket.SpendBox();
                    OnCharge?.Invoke();

                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
    }
}