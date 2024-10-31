using System;
using TMPro;
using UnityEngine;

namespace Modules
{
    public class HealthGateModule
    {
        private const string ColorText = "<#64b0ef>";
        private const int Damage = 1;
        
        private readonly int _maxHealth;
        private readonly int _minHealth;
        
        private int _currentHealth;
        private TMP_Text _healthView;

        public HealthGateModule(int maxHealthBaseGate, TMP_Text healthView)
        {
            _healthView = healthView;
            _maxHealth = maxHealthBaseGate;
            _currentHealth = _maxHealth;
            UpdateHealthView();
        }

        public event Action OnGateFall;
        
        public void ApplyDamage()
        {
            _currentHealth -= Mathf.Clamp(Damage, _minHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnGateFall?.Invoke();
            }
            
            UpdateHealthView();
        }

        private void UpdateHealthView() => 
            _healthView.text = $"{_currentHealth} {ColorText}/ {_maxHealth}";

        public void Reset()
        {
            _currentHealth = _maxHealth;
            UpdateHealthView();
        }
    }
}