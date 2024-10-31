using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsHaveGem : WorkerConditional
    {
        public override TaskStatus OnUpdate() =>
            Worker.IsHandEmpty == false ? TaskStatus.Success : TaskStatus.Failure;
    }
}