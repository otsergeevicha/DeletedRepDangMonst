using System;
using GameAnalyticsSDK;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.Inputs;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases.UpgradePlayer
{
    public class UpgradeHeroScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        
        [SerializeField] private TMP_Text _money;
        [SerializeField] private TMP_Text _timerView;

        [SerializeField] private GameObject _radialShine;
        [SerializeField] private Button _buttonGift;

        [SerializeField] private HeroLot[] _heroLots = new HeroLot[4];
        
        private readonly int _timerSeconds = 600;
        
        private IInputService _input;
        private bool _isGift;
        private float _timer;
        private IWallet _wallet;

        public event Action<string> OnCurrentTime;
        
        public void Construct(IInputService input, HeroData heroData, PriceListData priceList, IWallet wallet,
            Hero hero, ISDKService sdk)
        {
            _wallet = wallet;
            _input = input;
            _timer = _timerSeconds;

            foreach (HeroLot heroLot in _heroLots) 
                heroLot.Construct(heroData, priceList, wallet, hero, sdk, this);
        }

        private void Start() => 
            _canvas.enabled = false;

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        protected override void UpdateCached()
        {
            if (!_isGift)
            {
                _radialShine.SetActive(false);
                _buttonGift.interactable = false;
                
                if (_timer > 0)
                {
                    _timer -= Time.unscaledDeltaTime;
                    UpdateTimeView();
                }
                else
                {
                    _radialShine.SetActive(true);
                    _buttonGift.interactable = true;
                    _isGift = true;
                }
            }
        }

        public void OnActive()
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"UpgradeHeroScreen:OnActive");
#endif
            
            Time.timeScale = 0;
            _canvas.enabled = true;
            UpdateMoneyView(_wallet.ReadCurrentMoney());

            foreach (HeroLot lot in _heroLots)
            {
                lot.UpdatePriceView();
                lot.UpdateValueView();
            }
        }

        public void InActive()
        {
            Time.timeScale = 1;
            _canvas.enabled = false;
            _input.OnControls();
        }

        public void GetGift()
        {
            _timer = _timerSeconds;
            _isGift = false;

            foreach (HeroLot heroLot in _heroLots) 
                heroLot.MakeFree();
        }
        
        public void UpdateMoneyView(int currentMoney) => 
            _money.text = currentMoney.ToString();
        
        private void UpdateTimeView()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timer);
            
            string currentTime = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            OnCurrentTime?.Invoke(currentTime);
            _timerView.text = currentTime;
        }

        public void ReturnPrice()
        {
            foreach (HeroLot heroLot in _heroLots) 
                heroLot.ReturnPriceView();
        }
    }
}