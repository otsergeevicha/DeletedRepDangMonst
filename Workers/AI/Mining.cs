using Workers.AI.Parents;

namespace Workers.AI
{
    public class Mining : WorkerAction
    {
        public override void OnStart() => 
            Worker.WorkerAnimation.EnableMine();
    }
}