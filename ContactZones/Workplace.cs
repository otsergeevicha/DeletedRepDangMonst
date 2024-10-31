using Plugins.MonoCache;
using TMPro;
using UnityEngine;
using Workers;

namespace ContactZones
{
    public class Workplace : MonoCache
    {
        [SerializeField] private TMP_Text _slotData;
        
        private const string ColorText = "<#64b0ef>";
        private const string MaxText = "MAX";
        
        private int _currentCountWorker = 0;
        private int _maxWorkers;

        public void Construct(int maxCountWorkers)
        {
            _maxWorkers = maxCountWorkers;
            UpdateSlotText();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Worker worker))
            {
                worker.OnAtWork();
                _currentCountWorker++;
                UpdateSlotText();
            }
        }
        
        private void UpdateSlotText() =>
            _slotData.text = _currentCountWorker != _maxWorkers 
                ? $"{_currentCountWorker} {ColorText}/ {_maxWorkers}" 
                : $"{ColorText}{MaxText}";
    }
}