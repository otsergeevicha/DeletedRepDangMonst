using Agava.YandexGames;
using Plugins.MonoCache;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class BaseView : MonoCache
    {
        [SerializeField] private TMP_Text _slotData;

        private const string EngBase = "Base";
        private const string RuBase = "База";
        
        public void UpdateText(string currentLevel)
        {
#if !UNITY_EDITOR
            _slotData.text = YandexGamesSdk.Environment.i18n.lang == "en"
                ? (EngBase + " " + currentLevel)
                : (RuBase + " " + currentLevel);
            return;
#endif
            _slotData.text = EngBase + " " + currentLevel;
        }
    }
}