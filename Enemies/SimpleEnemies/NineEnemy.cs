namespace Enemies.SimpleEnemies
{
    public class NineEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.NineLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.NineEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.NineEnemyDamage;
    }
}