using System;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class SectionPlate : MonoCache
    {
        [SerializeField] private GameObject _section;
        [SerializeField] private GameObject[] _inActiveObjects;

        [SerializeField] private Image _background;
        
        [SerializeField] private TMP_Text _price;
        [SerializeField] private GameObject _ad;
        
        private readonly float _waitTime = 2f;

        private IWallet _wallet;
        private PriceListData _priceList;

        private bool _isWaiting;
        private float _currentFillAmount = 1f;
        private PoolData _poolData;
        private ISDKService _sdk;
        private int _currentPrice;

        public event Action OnNotifyAssistant;

        public void Construct(IWallet wallet, PriceListData priceListData, PoolData poolData, ISDKService sdk)
        {
            _sdk = sdk;
            _poolData = poolData;
            _priceList = priceListData;
            _wallet = wallet;

            _currentPrice = _poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier;
            
            UpdatePrice();
            _wallet.MoneyChanged += SetConfigurationPrice;
            SetConfigurationPrice(_wallet.ReadCurrentMoney());
        }

        protected override void OnDisabled() => 
            _wallet.MoneyChanged -= SetConfigurationPrice;

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
                ResetFill();
            }
        }

        protected override void UpdateCached()
        {
            if (_isWaiting)
            {
                _currentFillAmount -= Time.deltaTime / _waitTime;
                _background.fillAmount = _currentFillAmount;

                if (_currentFillAmount <= Single.Epsilon)
                {
                    FinishWaiting();
                    _isWaiting = false;
                    _currentFillAmount = 1f;
                    ResetFill();
                }
            }
        }

        public void UpdateLevel()
        {
            _section.SetActive(false);

            foreach (GameObject inActiveObject in _inActiveObjects)
                inActiveObject.SetActive(true);

            _currentPrice = _poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier;
            
            UpdatePrice();
            SetConfigurationPrice(_wallet.ReadCurrentMoney());
        }

        private void FinishWaiting()
        {
            if (_wallet.Check(_currentPrice))
            {
                _wallet.SpendMoney(_currentPrice);
                OnAdditionalSection();
                gameObject.SetActive(false);
            }
            else
            {
                _sdk.AdReward(delegate
                {
                    OnAdditionalSection();
                    gameObject.SetActive(false);
                });
            }
        }
        
        private void SetConfigurationPrice(int moneyAmount)
        {
            if (_currentPrice <= moneyAmount)
            {
                _ad.SetActive(false);
                _price.gameObject.SetActive(true);
            }
            else
            {
                _ad.SetActive(true);
                _price.gameObject.SetActive(false);
            }
        }

        private void OnAdditionalSection()
        {
            OnNotifyAssistant?.Invoke();
            
            _section.SetActive(true);

            foreach (GameObject inActiveObject in _inActiveObjects)
                inActiveObject.SetActive(false);
        }

        private void UpdatePrice() =>
            _price.text = $"{_currentPrice}";

        private void ResetFill() =>
            _background.fillAmount = 1;
    }
}