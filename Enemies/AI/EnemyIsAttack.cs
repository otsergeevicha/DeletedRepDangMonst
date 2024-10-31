using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyIsAttack : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsAgro ? TaskStatus.Success : TaskStatus.Failure;
    }
}