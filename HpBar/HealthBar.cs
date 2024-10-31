using System.Collections;
using System.Linq;
using Effects;
using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace HpBar
{
    public class HealthBar : MonoCache
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Slider _sliderHP;

        [SerializeField] private FloatingHitText[] _floatingHit = new FloatingHitText[5];

        private Transform _followingTransform;
        private Coroutine _coroutine;

        public void Construct(Transform following)
        {
            _followingTransform = following;
            _mainCanvas.enabled = false;

            foreach (FloatingHitText hitText in _floatingHit) 
                hitText.EndAnimation();
        }

        protected override void UpdateCached()
        {
            if (!_mainCanvas.enabled)
                return;

            transform.position = new Vector3(_followingTransform.position.x, 2.5f, _followingTransform.position.z);
        }

        public void InActive() => 
            _mainCanvas.enabled = false;

        public void ChangeValue(float current, float max, int damage)
        {
            _floatingHit.FirstOrDefault(text => 
                !text.IsActive)?.OnActive(damage);
            
            _mainCanvas.enabled = true;
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(UpdateView(current, max));
        }

        private IEnumerator UpdateView(float current, float max)
        {
            float targetValue = current / max;

            while (!Mathf.Approximately(_sliderHP.value, targetValue))
            {
                _sliderHP.value = Mathf.MoveTowards(_sliderHP.value, targetValue, .1f);
                yield return null;
            }
        }
    }
}