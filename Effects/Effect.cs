using Plugins.MonoCache;
using UnityEngine;

namespace Effects
{
    public abstract class Effect : MonoCache
    {
        [SerializeField] private ParticleSystem _particle;

        public void Construct()
        {
            var main = _particle.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        public void OnActive(Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
            gameObject.SetActive(true);
            _particle.Play();
        }

        public void InActive() => 
            gameObject.SetActive(false);

        private void OnParticleSystemStopped() => 
            InActive();
    }
}