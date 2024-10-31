using Plugins.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class LoadingScreen : MonoCache
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _percentView;

        private const float LoadingSeconds = 3f;
        private float _elapsed = 0f;
        private float _progress;

        private bool _isLoading = true;

        protected override void UpdateCached()
        {
            if (!_isLoading)
                return;

            _elapsed += Time.deltaTime;
            _progress = _elapsed / LoadingSeconds;

            if (_progress >= 1f)
                _isLoading = false;
            
            _slider.value = _progress;
            _percentView.text = (_progress * 100f).ToString("F0") + "%";
        }
    }
}