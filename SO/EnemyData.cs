using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Characters/Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [HideInInspector] public int WalkHash = Animator.StringToHash("Walk");
        [HideInInspector] public int AttackHash = Animator.StringToHash("Attack");
        [HideInInspector] public int HitHash = Animator.StringToHash("Hit");
        [HideInInspector] public int DeathHash = Animator.StringToHash("Death");

        [Header("Speed enemies")]
        [Range(1, 10)] 
        public int Speed = 5;
        
        [Header("Agro radius enemies")]
        [Range(5, 15)] 
        public int AgroDistance = 5;
        [Range(1, 5)] 
        public float AttackDistance = 1;
        [Range(1, 15)] 
        public float BossAttackDistance = 1;

        [Header("Health simple enemies")] 
        [Range(1, 50)]
        public int ZeroEnemyHealth = 1;
        [Range(1, 50)]
        public int OneEnemyHealth = 1;
        [Range(1, 50)]
        public int TwoEnemyHealth = 1;
        [Range(1, 50)]
        public int ThreeEnemyHealth = 1;
        [Range(1, 50)]
        public int FourEnemyHealth = 1;
        [Range(1, 50)]
        public int FiveEnemyHealth = 1;
        [Range(1, 50)]
        public int SixEnemyHealth = 1;
        [Range(1, 50)]
        public int SevenEnemyHealth = 1;
        [Range(1, 50)]
        public int EightEnemyHealth = 1;
        [Range(1, 50)]
        public int NineEnemyHealth = 1;
        
        [Header("Damage simple enemies")] 
        [Range(1, 50)]
        public int ZeroEnemyDamage = 1;
        [Range(1, 50)]
        public int OneEnemyDamage = 1;
        [Range(1, 50)]
        public int TwoEnemyDamage = 1;
        [Range(1, 50)]
        public int ThreeEnemyDamage = 1;
        [Range(1, 50)]
        public int FourEnemyDamage = 1;
        [Range(1, 50)]
        public int FiveEnemyDamage = 1;
        [Range(1, 50)]
        public int SixEnemyDamage = 1;
        [Range(1, 50)]
        public int SevenEnemyDamage = 1;
        [Range(1, 50)]
        public int EightEnemyDamage = 1;
        [Range(1, 50)]
        public int NineEnemyDamage = 1;
        
        [Header("Health boss enemies")] 
        [Range(1, 500)]
        public int OneBossHealth = 1;
        [Range(1, 500)]
        public int TwoBossHealth = 1;
        [Range(1, 500)]
        public int ThreeBossHealth = 1;
        [Range(1, 500)]
        public int FourBossHealth = 1;
        [Range(1, 500)]
        public int FiveBossHealth = 1;
        [Range(1, 500)]
        public int SixBossHealth = 1;
        [Range(1, 500)]
        public int SevenBossHealth = 1;
        [Range(1, 500)]
        public int EightBossHealth = 1;
        [Range(1, 500)]
        public int NineBossHealth = 1;
        [Range(1, 500)]
        public int TenBossHealth = 1;

        [Header("Damage boss enemies")] 
        [Range(1, 500)]
        public int OneBossDamage = 1;
        [Range(1, 500)]
        public int TwoBossDamage = 1;
        [Range(1, 500)]
        public int ThreeBossDamage = 1;
        [Range(1, 500)]
        public int FourBossDamage = 1;
        [Range(1, 500)]
        public int FiveBossDamage = 1;
        [Range(1, 500)]
        public int SixBossDamage = 1;
        [Range(1, 500)]
        public int SevenBossDamage = 1;
        [Range(1, 500)]
        public int EightBossDamage = 1;
        [Range(1, 500)]
        public int NineBossDamage = 1;
        [Range(1, 500)]
        public int TenBossDamage = 1;
    }
}