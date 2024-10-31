using Workers.AI.Parents;

namespace Workers.AI
{
    public class MoveWorkplace : WorkerAction
    {
        public override void OnStart()
        {
            Worker.WorkerAnimation.EnableRun();
            Agent.speed = Worker.WorkerData.Speed;
            Agent.destination = Worker.Workplace;
        }
    }
}