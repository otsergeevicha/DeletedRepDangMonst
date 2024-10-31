using Agava.YandexGames;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases.UpgradePlayer
{
    public abstract class HeroLot : MonoCache
    {
        [SerializeField] protected Button _activeButton;

        [SerializeField] protected TMP_Text ValueView;

        [SerializeField] protected TMP_Text PriceView;

        [SerializeField] protected GameObject Ad;

        [SerializeField] protected GameObject MaxLevel;
        [SerializeField] protected GameObject ReadyUpgrade;

        private const string EngFreeLot = "Free";
        private const string RuFreeLot = "Бесплатно";

        protected int CurrentValue;
        protected int CurrentPrice;

        protected HeroData HeroData;
        protected PriceListData PriceList;

        private UpgradeHeroScreen _upgradeHeroScreen;

        private IWallet _wallet;
        private int _tempPrice;
        private Hero _hero;
        private ISDKService _sdk;

        protected abstract void UpdatePrice();
        protected abstract void UpdateValue();
        protected abstract void Upgrade();
        protected abstract void IncreasePrice();
        protected abstract bool CheckUpperLimit();

        public void Construct(HeroData heroData, PriceListData priceList,
            IWallet wallet, Hero hero, ISDKService sdk, UpgradeHeroScreen upgradeHeroScreen)
        {
            _upgradeHeroScreen = upgradeHeroScreen;
            _sdk = sdk;
            _hero = hero;
            _wallet = wallet;
            PriceList = priceList;
            HeroData = heroData;

            UpdatePrice();
            UpdateValue();

            _wallet.MoneyChanged += SetConfigurationPrice;

            SetConfigurationValue();
            _tempPrice = CurrentPrice;
        }

        protected override void OnDisabled() =>
            _wallet.MoneyChanged -= SetConfigurationPrice;

        public void MakeFree()
        {
            if (!CheckUpperLimit())
            {
                _tempPrice = CurrentPrice;
                
                Ad.SetActive(false);
                PriceView.gameObject.SetActive(true);
                
                CurrentPrice = 0;
#if !UNITY_EDITOR
                PriceView.text = YandexGamesSdk.Environment.i18n.lang == "en" ? EngFreeLot : RuFreeLot;
                return;
#endif
                PriceView.text = RuFreeLot;
            }
        }

        public void Purchase()
        {
            if (CurrentPrice != 0)
                _tempPrice = CurrentPrice;

            if (_wallet.Check(CurrentPrice))
            {
                CurrentValue++;
                Upgrade();
                UpdateValue();

                _wallet.SpendMoney(CurrentPrice);

                CurrentPrice = _tempPrice;
                
                IncreasePrice();
                UpdatePrice();
                
                _tempPrice = CurrentPrice;

                _hero.Upgrade();
            }
            else
            {
                _sdk.AdReward(delegate
                {
                    CurrentValue++;
                    Upgrade();
                    UpdateValue();

                    _hero.Upgrade();
                });
            }

            _upgradeHeroScreen.ReturnPrice();
        }

        public void ReturnPriceView()
        {
            CurrentPrice = _tempPrice;
            UpdateValueView();
            UpdatePriceView();
            SetConfigurationPrice(_wallet.ReadCurrentMoney());
            SetConfigurationValue();
        }

        public void UpdatePriceView() =>
            PriceView.text = CurrentPrice.ToString();

        public void UpdateValueView() =>
            ValueView.text = CurrentValue.ToString();
        
        private void SetConfigurationValue()
        {
            if (CheckUpperLimit())
            {
                MaxLevel.SetActive(true);
                ReadyUpgrade.SetActive(false);

                _activeButton.interactable = false;
                Ad.SetActive(false);
                PriceView.gameObject.SetActive(false);
            }
            else
            {
                MaxLevel.SetActive(false);
                ReadyUpgrade.SetActive(true);

                _activeButton.interactable = true;
            }
        }

        private void SetConfigurationPrice(int moneyAmount)
        {
            if (CurrentPrice <= moneyAmount)
            {
                Ad.SetActive(false);
                PriceView.gameObject.SetActive(true);
            }
            else
            {
                Ad.SetActive(true);
                PriceView.gameObject.SetActive(false);
            }
        }
    }
}