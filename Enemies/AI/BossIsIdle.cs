using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class BossIsIdle : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsIdleBoss ? TaskStatus.Success : TaskStatus.Failure;
    }
}