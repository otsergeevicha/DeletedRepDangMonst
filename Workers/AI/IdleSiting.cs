using Workers.AI.Parents;

namespace Workers.AI
{
    public class IdleSiting : WorkerAction
    {
        public override void OnStart() => 
            Worker.WorkerAnimation.EnableSitingIdle();
    }
}