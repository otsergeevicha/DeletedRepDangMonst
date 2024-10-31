using System;
using Canvases;
using SO;
using UnityEngine;

namespace Modules
{
    public class HeroHealthModule
    {
        private int _maxHealth;
        private int _minHealth = 0;
        private int _currentHealth;
        private HeroData _heroData;
        private HealthView _healthView;

        public HeroHealthModule(HealthView healthView, HeroData heroData)
        {
            _healthView = healthView;
            _heroData = heroData;
            
            Reset();
        }

        public event Action Died;

        public void ApplyDamage(int damage)
        {
            _currentHealth -= Mathf.Clamp(damage, _minHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Died?.Invoke();
            }
            
            _healthView.ChangeValue(_currentHealth, _maxHealth, damage);
        }

        public void Reset()
        {
            _healthView.HealingValue();
            _maxHealth = _heroData.MaxHealth;
            _currentHealth = _heroData.MaxHealth;
        }
    }
}