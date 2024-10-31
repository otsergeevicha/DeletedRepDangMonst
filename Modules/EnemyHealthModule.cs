namespace Modules
{
    public class EnemyHealthModule
    {
        public int CalculateDamage(int currentHealth, int damage) => 
            currentHealth - damage;
    }
}