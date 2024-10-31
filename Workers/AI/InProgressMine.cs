using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class InProgressMine : WorkerConditional
    {
        public override TaskStatus OnUpdate() =>
            Worker.IsProcessMining ? TaskStatus.Success : TaskStatus.Failure;
    }
}