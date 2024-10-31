using System;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Loots
{
    public class MedicalBox : MonoCache, ILoot
    {
        [Header("Animators")] 
        [SerializeField] private Animator _animatorBody;
        [SerializeField] private Animator _animatorCap;

        private Hero _hero;

        public void Construct(Hero hero) => 
            _hero = hero;

        public void OnActive()
        {
            gameObject.SetActive(true);
            _animatorBody.enabled = true;
        }

        public void Open(Action opened)
        {
            _hero.OnHealing();
            opened?.Invoke();
            _animatorBody.enabled = false;
        }

        public string GetName() => 
            "MedicalBox";

        public void InActive()
        {
            _animatorBody.enabled = true;
            gameObject.SetActive(false);
        }
    }
}