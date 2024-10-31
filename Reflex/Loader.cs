using System.Collections;
using Agava.YandexGames;
using GameAnalyticsSDK;
using Plugins.MonoCache;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflex
{
    public class Loader : MonoCache
    {
        private IEnumerator Start()
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
            
            Debug.Log("Imitation SDK initialized");
            LaunchGame();
            yield break;
#endif
            GameAnalytics.Initialize();           

            yield return YandexGamesSdk.Initialize(LaunchGame);
            Debug.Log("SDK initialized");

            if (PlayerAccount.IsAuthorized)
                PlayerAccount.GetCloudSaveData(OnSuccessCallback, OnErrorCallback);
            else
                LaunchGame();
        }

        private void OnSuccessCallback(string data)
        {
            PlayerPrefs.SetString(Constants.Progress, data);
            PlayerPrefs.Save();
            
            LaunchGame();
        }

        private void OnErrorCallback(string _) => 
            LaunchGame();

        private void LaunchGame()
        {
             Scene scene = SceneManager.LoadScene("Main", new LoadSceneParameters(LoadSceneMode.Single));
             ReflexSceneManager.PreInstallScene(scene, builder => builder.AddSingleton(""));
        }
    }
}