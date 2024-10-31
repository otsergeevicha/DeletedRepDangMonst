using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Workers.AI.Parents
{
    public class WorkerAction : Action
    {
        protected Worker Worker;
        protected NavMeshAgent Agent;

        public override void OnAwake()
        {
            Worker ??= GetComponent<Worker>();
            Agent ??= GetComponent<NavMeshAgent>();
        }
    }
}