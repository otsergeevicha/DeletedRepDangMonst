using System;
using Agava.YandexGames;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class YandexSdk
    {
        private AudioSource _audioSource;

        public void Inject(AudioSource audioSource) => 
            _audioSource = audioSource;

        public void ShowReward(Action onCompleted)
        {
            VideoAd.Show(Mute
                , () =>
                {
                    onCompleted?.Invoke();
                    UnMute();
                }, UnMute
                , (string _) => { UnMute(); });
        }

        public void InterstitialAd(Action action)
        {
            Agava.YandexGames.InterstitialAd.Show(Mute
                , (bool _) => action?.Invoke()
                , (string _) =>
                {
                    action?.Invoke();
                    UnMute();
                }, () =>
                {
                    action?.Invoke();
                    UnMute();
                });
        }

        private void Mute()
        {
            Time.timeScale = 0;
            _audioSource.volume = 0;
        }

        private void UnMute()
        {
            Time.timeScale = 1;
            _audioSource.volume = 1;
        }
    }
}