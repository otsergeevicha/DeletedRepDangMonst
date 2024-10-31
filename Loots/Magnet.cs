using System;
using Player;
using Plugins.MonoCache;

namespace Loots
{
    public class Magnet : MonoCache, ILoot
    {
        private Hero _hero;

        public void Construct(Hero hero) => 
            _hero = hero;

        public void OnActive() => 
            gameObject.SetActive(true);

        public void Open(Action opened)
        {
            _hero.OnMagnetEffect();
            opened?.Invoke();
        }

        public string GetName() => 
            "Magnet";

        public void InActive() => 
            gameObject.SetActive(false);
    }
}