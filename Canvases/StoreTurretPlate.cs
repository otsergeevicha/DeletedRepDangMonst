using System;
using System.Linq;
using GameAnalyticsSDK;
using Infrastructure.Factory.Pools;
using Modules;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using TMPro;
using Turrets;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class StoreTurretPlate : MonoCache, ITutorialPlate
    {
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private Transform _markerPosition;

        [SerializeField] private TMP_Text _priceView;
        [SerializeField] private GameObject _ad;

        [SerializeField] private Image _background;
        [SerializeField] private Transform _spawnPoint;

        [Header("First")] [SerializeField] private Image _iconAdd;

        [Header("Second")] [SerializeField] private GameObject _iconUpgrade;

        private readonly float _waitTime = 2f;

        private bool _isWaiting;
        private float _currentFillAmount = 1f;
        private PoolTurrets _poolTurrets;
        private bool _purchased;
        private Turret _turret;

        private IWallet _wallet;
        private PriceListData _priceListData;
        private ISDKService _sdk;
        private PoolData _poolData;

        public event Action OnTutorialContacted;

        public void Construct(PoolTurrets poolTurrets, IWallet wallet, PriceListData priceListData,
            ISDKService sdk, PoolData poolData)
        {
            _poolData = poolData;
            _sdk = sdk;
            _priceListData = priceListData;
            _wallet = wallet;
            _poolTurrets = poolTurrets;
            ResetFill();

            _wallet.MoneyChanged += SetConfigurationPrice;
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

        public Transform GetRootCamera() =>
            _rootCamera;

        public Vector3 GetPositionMarker() =>
            _markerPosition.transform.position;

        public void UpdateLevel()
        {
            _purchased = false;
            SetConfigurationPrice(_wallet.ReadCurrentMoney());
        }

        private void FinishWaiting()
        {
            if (_purchased && _turret != null)
            {
                if (_wallet.Check(_turret.GetPrice()))
                {
                    _wallet.SpendMoney(_turret.GetPrice());
                    _turret.IncreasePrice(_priceListData.StepIncreasePriceTurret);
                    _turret.Upgrade();
                }
                else
                {
                    _sdk.AdReward(() =>
                        _turret.Upgrade());
                }

                UpdatePriceView();
                SetConfigurationPrice(_wallet.ReadCurrentMoney());
                
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Turret:Upgrade:OnLevel - {_poolData.CurrentLevelGame}");
#endif
                return;
            }

            if (!_purchased)
            {
                _turret = _poolTurrets.Turrets.FirstOrDefault(turret =>
                    turret.isActiveAndEnabled == false);

                if (_turret != null)
                    _turret.OnActive(_spawnPoint, _priceListData.StartPriceTurret);

                _purchased = true;
                OnTutorialContacted?.Invoke();

                SetConfigurationPrice(_wallet.ReadCurrentMoney());
                
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Turret:Purchased:OnLevel - {_poolData.CurrentLevelGame}");
#endif
            }

            _iconAdd.gameObject.SetActive(false);
            _iconUpgrade.gameObject.SetActive(true);
        }

        private void SetConfigurationPrice(int moneyAmount)
        {
            if (GetCurrentPrice() <= moneyAmount)
            {
                _ad.SetActive(false);
                _priceView.gameObject.SetActive(true);
                UpdatePriceView();
            }
            else
            {
                _ad.SetActive(true);
                _priceView.gameObject.SetActive(false);
            }
        }

        private int GetCurrentPrice() =>
            _purchased ? _turret.GetPrice() : _priceListData.StartPriceTurret;

        private void UpdatePriceView() =>
            _priceView.text = GetCurrentPrice().ToString();

        private void ResetFill() =>
            _background.fillAmount = 1;
    }
}