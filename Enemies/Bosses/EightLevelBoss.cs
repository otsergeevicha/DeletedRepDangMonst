namespace Enemies.Bosses
{
    public class EightLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.EightLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.EightBossHealth;

        protected override void SetCurrentDamage() => 
            Damage = EnemyData.EightBossDamage;
    }
}