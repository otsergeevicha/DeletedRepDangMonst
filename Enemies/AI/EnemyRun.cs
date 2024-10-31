using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyRun : EnemyAction
    {
        public override void OnStart()
        {
            Enemy.EnemyAnimation.EnableRun();
            Agent.speed = Enemy.EnemyData.Speed;
            Agent.destination = Enemy.GetCurrentTarget.position;
        }
    }
}