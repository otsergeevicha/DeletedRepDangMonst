using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

namespace Modules
{
    public class LanguageModule : MonoBehaviour
    {
        private const string RussianLanguage = "Russian";
        private const string EnglishLanguage = "English";

        private string _currentLanguage;

        private void Start() => 
            SetLanguage();
        
        private void SetLanguage()
        {
#if UNITY_EDITOR
            LeanLocalization.SetCurrentLanguageAll(RussianLanguage);
            return;
#endif
            
            switch (YandexGamesSdk.Environment.i18n.lang)
            {
                case "en":
                    LeanLocalization.SetCurrentLanguageAll(EnglishLanguage);
                    break;
                case "ru":
                    LeanLocalization.SetCurrentLanguageAll(RussianLanguage);
                    break;
                default:
                    LeanLocalization.SetCurrentLanguageAll(EnglishLanguage);
                    break;
            }
        }
    }
}