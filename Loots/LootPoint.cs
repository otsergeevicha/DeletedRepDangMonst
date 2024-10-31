using System;
using GameAnalyticsSDK;
using Player;
using Plugins.MonoCache;
using Services.SDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Loots
{
    enum TypeLoot
    {
        Money,
        Gem,
        Magnet,
        Health
    }
    
    public class LootPoint : MonoCache
    {
        private const int MaxInclusive = 100;
        
        [HideInInspector] [SerializeField] private Magnet _magnet;
        [HideInInspector] [SerializeField] private MedicalBox _medicalBox;
        [HideInInspector] [SerializeField] private RandomBox _randomBox;

        [SerializeField] private GameObject _markerParticle;
        [SerializeField] private GameObject _adView;
        [SerializeField] private GameObject _freeView;
        [SerializeField] private Image _confirmationBar;

        [SerializeField] private TMP_Text _timerView;

        [Header("Time waiting in seconds")] 
        [SerializeField] private int _timerSeconds = 60;
        
        private readonly float _waitTime = 2f;
        
        private ISDKService _sdkService;
        private ILoot _actualLoot;
        
        private bool _isFreeLoot;
        private bool _isWaiting;
        private float _timer;
        private float _currentFillAmount = 1f;

        public void Construct(ISDKService sdkService, Hero hero)
        {
            _sdkService = sdkService;

            _magnet.Construct(hero);
            _medicalBox.Construct(hero);
            _randomBox.Construct(hero);
        }

        public event Action OnPickUp;
        
        private void OnValidate()
        {
            _magnet ??= ChildrenGet<Magnet>();
            _medicalBox ??= ChildrenGet<MedicalBox>();
            _randomBox ??= ChildrenGet<RandomBox>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            OnConfirmationBar();
            _confirmationBar.fillAmount = 1;

            if (collision.TryGetComponent(out Hero _))
            {
                if (!_isFreeLoot)
                {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Loot:PickUp:NotFree:{_actualLoot.GetName()}");
#endif
                    _sdkService.AdReward(() => 
                        _actualLoot.Open(InActive));
                }
                else
                {
                    _isWaiting = true;
                }
                
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            OffConfirmationBar();
            _confirmationBar.fillAmount = 1;
            
            if (collision.TryGetComponent(out Hero _))
            {
                if (_isFreeLoot)
                {
                    _isWaiting = false;
                    _currentFillAmount = 1;
                }
            }
        }

        protected override void UpdateCached()
        {
            if (_isFreeLoot)
            {
                if (_timer > 0)
                {
                    _timer -= Time.unscaledDeltaTime;
                    UpdateTimeView();
                }
                else
                {
                    _isFreeLoot = false;
                    InActive();
                }
            }
            
            if (_isWaiting)
            {
                _currentFillAmount -= Time.deltaTime / _waitTime;
                _confirmationBar.fillAmount = _currentFillAmount;

                if (_currentFillAmount <= Single.Epsilon)
                {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Loot:PickUp:Free:{_actualLoot.GetName()}");
#endif
                    _actualLoot.Open(InActive);
                    _isWaiting = false;
                    _currentFillAmount = 1f;
                }
            }
        }

        public void OnActive(int currentLoot, int percentFreeChance)
        {
            _magnet.InActive();
            _medicalBox.InActive();
            _randomBox.InActive();
            
            gameObject.SetActive(true);
            
            if (GetRandom(percentFreeChance))
            {
                _timer = _timerSeconds;
                
                _isFreeLoot = true;
                FreeConfig();
                OnCurrentLoot(currentLoot);
            }
            else
            {
                _isFreeLoot = false;
                PayConfig();
                OnCurrentLoot(currentLoot);
            }
        }

        public void InActive()
        {
            _freeView.gameObject.SetActive(true);
            _confirmationBar.enabled = true;
            _adView.gameObject.SetActive(true);

            gameObject.SetActive(false);
            OnPickUp?.Invoke();
        }

        private void OnConfirmationBar()
        {
            _markerParticle.gameObject.SetActive(false);
            _confirmationBar.enabled = true;
        }

        private void OffConfirmationBar()
        {
            _markerParticle.gameObject.SetActive(true);
            _confirmationBar.enabled = false;
        }

        private void UpdateTimeView()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timer);
            _timerView.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        private void PayConfig() => 
            _freeView.gameObject.SetActive(false);

        private void FreeConfig()
        {
            _confirmationBar.enabled = false;
            _adView.gameObject.SetActive(false);
        }

        private void OnCurrentLoot(int currentLoot)
        {
            switch (currentLoot)
            {
                case (int)TypeLoot.Money:
                    _randomBox.OnActive(currentLoot);
                    _actualLoot = _randomBox;
                    break;

                case (int)TypeLoot.Gem:
                    _randomBox.OnActive(currentLoot);
                    _actualLoot = _randomBox;
                    break;

                case (int)TypeLoot.Magnet:
                    _magnet.OnActive();
                    _actualLoot = _magnet;
                    break;

                case (int)TypeLoot.Health:
                    _medicalBox.OnActive();
                    _actualLoot = _medicalBox;
                    break;

                default:
                    _medicalBox.OnActive();
                    _actualLoot = _medicalBox;
                    Debug.Log("Incorrect choice loot spawn");
                    break;
            }
        }

        private bool GetRandom(int percentFreeChance) => 
            Random.Range(Single.Epsilon, MaxInclusive) <= percentFreeChance;
    }
}