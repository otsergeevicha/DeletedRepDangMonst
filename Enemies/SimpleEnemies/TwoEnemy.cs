namespace Enemies.SimpleEnemies
{
    public class TwoEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.TwoLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.TwoEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.TwoEnemyDamage;
    }
}