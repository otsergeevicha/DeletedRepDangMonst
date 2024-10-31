using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class BossIdle : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableIdle();
    }
}