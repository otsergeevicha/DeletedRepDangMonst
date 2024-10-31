using Agava.YandexGames;
using Services.SaveLoad;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace SaveLoadModule
{
    public class SaveLoad : ISave
    {
        private readonly Progress _progress;

        public SaveLoad()
        {
            _progress = PlayerPrefs.HasKey(Constants.Progress)
                ? JsonUtility.FromJson<Progress>(PlayerPrefs.GetString(Constants.Progress))
                : new Progress();
        }

        public Progress AccessProgress() =>
            _progress;

        public void Save()
        {
            string data = JsonUtility.ToJson(_progress);

            RecordToPrefs(data);

#if !UNITY_EDITOR
            if (PlayerAccount.IsAuthorized) 
                PlayerAccount.SetCloudSaveData(data);
#endif
        } 
        
        private void RecordToPrefs(string data)
        {
            PlayerPrefs.SetString(Constants.Progress, data);
            PlayerPrefs.Save();
        }
    }
}