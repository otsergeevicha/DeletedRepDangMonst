using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Markers
{
    public class HeroAimRing : MonoCache
    {
        [SerializeField] private ParticleSystem _particleOne;
        [SerializeField] private ParticleSystem _particleTwo;

        public MagnetEffect MagnetEffect;
        
        private Transform _following;
        private float _heroDataRadiusDetection;

        public void Construct(Transform heroTransform, float heroDataRadiusDetection)
        {
            _heroDataRadiusDetection = heroDataRadiusDetection/4;
            _following = heroTransform;

            InActive();
        }

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            transform.position = new Vector3(_following.position.x, 1f, _following.position.z);
        }

        public void OnActive()
        {
            gameObject.SetActive(true);
            _particleOne.startSize = _heroDataRadiusDetection;
            _particleTwo.startSize = _heroDataRadiusDetection;
        }

        public void InActive() => 
            gameObject.SetActive(false);

        public void ChangeRadius(float newRadius) => 
            _heroDataRadiusDetection = newRadius / 2;
    }
}