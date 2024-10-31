using System.Collections;
using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class HealthView : MonoCache
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _sliderHP;
        [SerializeField] private Animation _animation;
        
        private Coroutine _coroutine;

        private void Start() => 
            _canvas.enabled = false;

        public void ChangeValue(float current, float max, int damage)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(UpdateView(current, max));
        }
        
        public void HealingValue()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(UpdateView(1f, 1f));
        }
        
        private IEnumerator UpdateView(float current, float max)
        {
            float targetValue = current / max;

            while (!Mathf.Approximately(_sliderHP.value, targetValue))
            {
                _sliderHP.value = Mathf.MoveTowards(_sliderHP.value, targetValue, Time.deltaTime * 1.5f);
                yield return null;
            }
        }

        public void ActiveHeartAnimation(bool flag)
        {
            if (flag) 
                _animation.Play();
            else
                _animation.Stop();
        }

        public void ChangeActive(bool flag) => 
            _canvas.enabled = flag;
    }
}