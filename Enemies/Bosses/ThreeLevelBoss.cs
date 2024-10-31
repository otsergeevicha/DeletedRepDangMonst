namespace Enemies.Bosses
{
    public class ThreeLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.ThreeLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.ThreeBossHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.ThreeBossDamage;
    }
}