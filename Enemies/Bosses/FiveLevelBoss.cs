namespace Enemies.Bosses
{
    public class FiveLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.FiveLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.FiveBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.FiveBossDamage;
    }
}