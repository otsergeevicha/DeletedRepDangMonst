using Workers.AI.Parents;

namespace Workers.AI
{
    public class DeliveryGem : WorkerAction
    {
        public override void OnStart()
        {
            Worker.WorkerAnimation.EnableWalk();
            Agent.speed = Worker.WorkerData.Speed;
            Agent.destination = Worker.StorageGemPoint;
        }
    }
}