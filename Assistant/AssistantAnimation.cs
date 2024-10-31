using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Assistant
{
    [RequireComponent(typeof(Animator))]
    public class AssistantAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private AssistantData _assistantData;

        public void Construct(AssistantData assistantData) => 
            _assistantData = assistantData;

        private void OnValidate() => 
            _animator ??= Get<Animator>();
        
        public void EnableRun()
        {
            _animator.SetBool(_assistantData.IdleHash, false);
            _animator.SetBool(_assistantData.RunHash, true);
        }
        
        public void EnableIdle()
        {
            _animator.SetBool(_assistantData.RunHash, false);
            _animator.SetBool(_assistantData.IdleHash, true);
        }
    }
}