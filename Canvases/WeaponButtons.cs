using System;
using GameAnalyticsSDK;
using Plugins.MonoCache;
using Services.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    enum WeaponType
    {
        Uzi,
        AutoPistol,
        Rifle,
        MiniGun
    }

    public class WeaponButtons : MonoCache
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Color[] _colors = new Color[2];
        [SerializeField] private Image[] _icons = new Image[4];
        [SerializeField] private TMP_Text[] _countBullets = new TMP_Text[4];

        private const string ColorText = "<#CF4D4D>";

        public event Action<int> OnChanged;

        public void Construct(IInputService inputService)
        {
            inputService.SelectUzi(ActiveGun);
            inputService.SelectAutoPistol(ActiveGun);
            inputService.SelectRifle(ActiveGun);
            inputService.SelectMiniGun(ActiveGun);
        }

        public void ChangeActive(bool flag)
        {
            _canvas.enabled = flag;

            if (flag) 
                ActiveGun((int)WeaponType.Uzi);
        }

        public void ActiveGun(int currentGun)
        {
            foreach (Image icon in _icons) 
                icon.color = _colors[0];

            foreach (TMP_Text countBullet in _countBullets) 
                countBullet.enabled = false;

            switch (currentGun)
            {
                case (int)WeaponType.Uzi:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.AutoPistol:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.MiniGun:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.Rifle:
                    Notify(currentGun);
                    break;
                default:
                    Notify(currentGun);
                    break;
            }
        }

        public void UpdateViewBullets(int indexWeapon, int currentCount, int maxCount)
        {
            if (_countBullets[indexWeapon].enabled)
                _countBullets[indexWeapon].text = $"{currentCount} {ColorText}/ {maxCount}";
        }

        private void Notify(int currentGun)
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"SelectedWeapon::{currentGun}");
#endif
            
            _icons[currentGun].color = _colors[1];
            _countBullets[currentGun].enabled = true;
            OnChanged?.Invoke(currentGun);
        }
    }
}