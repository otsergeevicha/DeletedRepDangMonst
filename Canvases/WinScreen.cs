using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class WinScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _buttonContinue;

        [SerializeField] private TMP_Text _remainingMoney;
        [SerializeField] private TMP_Text _remainingGem;

        private IWallet _wallet;
        private ISDKService _sdk;
        private bool _isAdShowed;

        public void Construct(IWallet wallet, ISDKService sdk)
        {
            _sdk = sdk;
            _wallet = wallet;
        }

        private void Start()
        {
            _canvas.enabled = false;
            _buttonContinue.SetActive(false);
        }

        private void OnValidate() =>
            _canvas ??= Get<Canvas>();

        public void RewardX2()
        {
            _sdk.AdReward(delegate
            {
                _wallet.ApplyMoney(_wallet.ReadCurrentMoney());
                _wallet.ApplyGem(_wallet.ReadCurrentGem());

                _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
                _remainingGem.text = _wallet.ReadCurrentGem().ToString();

                _isAdShowed = true;
            });
        }

        public void OnActive()
        {
            Time.timeScale = 0;
            _canvas.enabled = true;

            _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
            _remainingGem.text = _wallet.ReadCurrentGem().ToString();
        }

        public void InActive()
        {
            if (_isAdShowed == false)
                _sdk.ShowInterstitial(ContinueGame);
            else
                ContinueGame();

            _isAdShowed = false;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1;
            _buttonContinue.SetActive(false);
            _canvas.enabled = false;
        }

        public void ActiveButtonContinue() =>
            _buttonContinue.SetActive(true);
    }
}