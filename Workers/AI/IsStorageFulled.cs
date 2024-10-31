using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsStorageFulled : WorkerConditional
    {
        public override TaskStatus OnUpdate() =>
            Worker.IsStorageFulled ? TaskStatus.Failure : TaskStatus.Success;
    }
}