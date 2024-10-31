namespace Enemies.Bosses
{
    public class SevenLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.SevenLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.SevenBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.SevenBossDamage;
    }
}