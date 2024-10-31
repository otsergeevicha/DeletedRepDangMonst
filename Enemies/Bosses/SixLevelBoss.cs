namespace Enemies.Bosses
{
    public class SixLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.SixLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.SixBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.SixBossDamage;
    }
}