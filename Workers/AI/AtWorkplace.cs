using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class AtWorkplace : WorkerConditional
    {
        public override TaskStatus OnUpdate() => 
            Worker.AtWork ? TaskStatus.Success : TaskStatus.Failure;
    }
}