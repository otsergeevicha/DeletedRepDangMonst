namespace Enemies.SimpleEnemies
{
    public class OneEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.OneLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.OneEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.OneEnemyDamage;
    }
}