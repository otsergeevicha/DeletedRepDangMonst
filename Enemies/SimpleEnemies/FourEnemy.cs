namespace Enemies.SimpleEnemies
{
    public class FourEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.FourLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.FourEnemyHealth;
        
        protected override void SetCurrentDamage() => 
            Damage = EnemyData.FourEnemyDamage;
    }
}