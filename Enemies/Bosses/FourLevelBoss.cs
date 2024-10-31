namespace Enemies.Bosses
{
    public class FourLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.FourLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.FourBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.FourBossDamage;
    }
}