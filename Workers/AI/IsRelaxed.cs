using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsRelaxed : WorkerConditional
    {
        public override TaskStatus OnUpdate()
        {
            if (!Worker.IsHandEmpty && Worker.IsStorageFulled && Worker.AtWork)
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }
    }
}