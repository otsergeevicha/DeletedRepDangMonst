using Plugins.MonoCache;
using UnityEngine;

namespace Loots
{
    public class Money : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private readonly int _pickUpHash = Animator.StringToHash("PickUp");
        
        public int CurrentNominal { get; private set; } = 1;

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void OnActive(int enemyId, Vector3 newPosition)
        {
            if (newPosition.y < 0) 
                newPosition.y = 0;

            transform.position = newPosition;
            SetNominal(enemyId);
            gameObject.SetActive(true);
        }

        public void PickUp() => 
            _animator.SetTrigger(_pickUpHash);

        private void EndPickUp()
        {
            ResetNominal();
            _animator.ResetTrigger(_pickUpHash);
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(false);
        }

        private void SetNominal(int enemyId) => 
            CurrentNominal = (enemyId + 1) * 5;

        private void ResetNominal() => 
            CurrentNominal = 1;
    }
}