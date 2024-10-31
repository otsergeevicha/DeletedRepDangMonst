using System;
using Services.SDK;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class SDKService : ISDKService
    {
        private readonly YandexSdk _yandex = new();

        public void Inject(AudioSource audioSource) => 
            _yandex.Inject(audioSource);

        public void AdReward(Action rewardCompleted)
        {
#if UNITY_EDITOR
            rewardCompleted?.Invoke();
            Debug.Log("Reward showed");
            return;
#endif

            _yandex.ShowReward(() =>
                rewardCompleted?.Invoke());
        }

        public void ShowInterstitial(Action adCompleted)
        {
#if UNITY_EDITOR
            adCompleted?.Invoke();
            Debug.Log("Interstitial showed");
            return;
#endif
            _yandex.InterstitialAd(()=>
                adCompleted?.Invoke());
        }
    }
}