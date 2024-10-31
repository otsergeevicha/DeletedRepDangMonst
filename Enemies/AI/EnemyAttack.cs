using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyAttack : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableAttack();
    }
}