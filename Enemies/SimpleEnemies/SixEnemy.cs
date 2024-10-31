namespace Enemies.SimpleEnemies
{
    public class SixEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.SixLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.SixEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.SixEnemyDamage;
    }
}