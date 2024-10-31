using System;
using UnityEngine;

namespace Services.SDK
{
    public interface ISDKService
    {
        void AdReward(Action rewardCompleted);
        void ShowInterstitial(Action adCompleted);
        void Inject(AudioSource audioSource);
    }
}