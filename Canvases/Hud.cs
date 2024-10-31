using System;
using CameraModule;
using Canvases.UpgradePlayer;
using ContactZones;
using JoystickLogic;
using Plugins.MonoCache;
using Services.Inputs;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class Hud : MonoCache
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private HealthView _heroHealthView;

        [SerializeField] private Joystick _joystick;
        [SerializeField] private TMP_Text _money;
        [SerializeField] private TMP_Text _gem;

        [SerializeField] private Canvas _weaponReload;

        [SerializeField] private TMP_Text _monsterEscapeText;
        
        [SerializeField] private GameObject _tutorialView;
        
        [SerializeField] private Canvas _canvasWallet;
        [SerializeField] private Canvas _canvasMonsterEscape;
        [SerializeField] private Canvas _canvasGiftTimer;

        [SerializeField] private TMP_Text _timer;
        
        public WeaponButtons WeaponButtons;

        private const int MaxCountEscape = 30;
        private const string ColorText = "<#64b0ef>";
    
        private MonstersPortal _monstersPortal;
        private int _countMonsters;
        private CameraFollow _cameraFollow;
        private Transform _rootUpgradePlayerBoard;

        public event Action OnGameOver;

        public HealthView GetHeroHealthView =>
            _heroHealthView;

        public void Construct(Camera mainCamera, MonstersPortal monstersPortal, IInputService inputService,
            UpgradeHeroScreen upgradeHeroScreen, CameraFollow cameraFollow, Transform rootUpgradePlayerBoard)
        {
            _rootUpgradePlayerBoard = rootUpgradePlayerBoard;
            _cameraFollow = cameraFollow;
            _monstersPortal = monstersPortal;

            _joystick.Construct(mainCamera, inputService);
            UpdateText();
            _monstersPortal.OnEscaped += MonsterEscaped;
            WeaponButtons.ChangeActive(false);
            WeaponButtons.Construct(inputService);

            upgradeHeroScreen.OnCurrentTime += timerText => 
                _timer.text = timerText;
        }

        protected override void OnDisabled() =>
            _monstersPortal.OnEscaped -= MonsterEscaped;

        public void OnActive() =>
            _canvas.enabled = true;
    
        public void InActive() =>
            _canvas.enabled = false;

        public void WeaponReload(bool flag) =>
            _weaponReload.enabled = flag;

        public void UpdateMoneyView(int currentMoney) =>
            _money.text = currentMoney.ToString();

        public void UpdateGemView(int currentGem) =>
            _gem.text = currentGem.ToString();

        public void HealthView(bool flag)
        {
            WeaponButtons.ChangeActive(flag);
            _heroHealthView.ActiveHeartAnimation(flag);
            _heroHealthView.ChangeActive(flag);
        }
        
        public void UpdateMonstersCounter()
        {
            _countMonsters = 0;
            UpdateText();
        }

        public void SetTutorialView(bool flag)
        {
            _canvasWallet.enabled = !flag;
            _canvasMonsterEscape.enabled = !flag;
            _canvasGiftTimer.enabled = !flag;
            _tutorialView.SetActive(flag);
        }

        public void OnClickGift() => 
            _cameraFollow.ShowMarker(_rootUpgradePlayerBoard);

        private void MonsterEscaped()
        {
            _countMonsters++;

            if (_countMonsters >= MaxCountEscape)
            {
                OnGameOver?.Invoke();
                _countMonsters = 0;
                UpdateText();
            }
            else
            {
                UpdateText();
            }
        }

        private void UpdateText() =>
            _monsterEscapeText.text
                = $"{_countMonsters} {ColorText}/ {MaxCountEscape}";
    }
}   