using Workers.AI.Parents;

namespace Workers.AI
{
    public class Idle : WorkerAction
    {
        public override void OnStart() => 
            Worker.WorkerAnimation.EnableIdle();
    }
}