using Plugins.MonoCache;
using TMPro;
using UnityEngine;

namespace Effects
{
    public class FloatingHitText : MonoCache
    {
        [SerializeField] private Animation _animation;
        
        [SerializeField] private Canvas _canvas;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color[] _colors = new Color[5];
        
        private readonly float _offSetY = .5f;

        public bool IsActive =>
            _canvas.enabled;
        
        public void OnActive(int damage)
        {
            _canvas.enabled = true;

            _text.text = (damage * Random.Range(10, 30)).ToString();
            _text.color = _colors[Random.Range(0, _colors.Length)];
            
            transform.localPosition += new Vector3(0f, _offSetY, 0f);

            _animation.Play();
        }
        
        public void EndAnimation()
        {
            _canvas.enabled = false;
            _animation.Stop();
        }
    }
}