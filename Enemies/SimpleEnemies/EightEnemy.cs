namespace Enemies.SimpleEnemies
{
    public class EightEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.EightLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.EightEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.EightEnemyDamage;
    }
}