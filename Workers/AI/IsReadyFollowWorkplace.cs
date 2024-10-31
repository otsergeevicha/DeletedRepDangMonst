using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsReadyFollowWorkplace : WorkerConditional
    {
        public override TaskStatus OnUpdate()
        {
            if (Worker.IsReadyWork && !Worker.AtWork)
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }
    }
}