using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewHero", menuName = "Characters/Hero", order = 1)]
    public class HeroData : ScriptableObject
    {
        [Range(1, 5000)]
        public int MaxHealth = 1;
        public readonly int UpperLimitMaxHealth = 5000;
        
        [Range(3, 8)]
        public int Speed = 3;
        public readonly int UpperLimitSpeed = 8;

        [Range(5, 15)] 
        public int SizeBasket = 5;
        public readonly int UpperLimitBasket = 15;
        
        [Header("Detection radius")]
        [Range(5f, 25f)]
        public float RadiusDetection = 5f;
        public readonly float UpperLimitRadiusDetection = 25f;
        
        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
        [HideInInspector] public int RunGunHash = Animator.StringToHash("RunGun");
        [HideInInspector] public int IdleAimingHash = Animator.StringToHash("IdleAiming");
    }
}