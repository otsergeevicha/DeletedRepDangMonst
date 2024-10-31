namespace Enemies.Bosses
{
    public class TenLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.TenLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.TenBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.TenBossDamage;
    }
}