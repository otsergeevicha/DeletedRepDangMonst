using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;
using UnityEngine;

namespace Enemies.AI
{
    public class EnemyIsRunning : EnemyConditional
    {
        public override TaskStatus OnUpdate()
        {
            float currentDistance = Vector3.Distance(Enemy.CashTransform.position, Enemy.GetCurrentTarget.position);

            return currentDistance > Enemy.CurrentAttackDistance
                   || currentDistance > Enemy.CurrentAgroDistance
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}