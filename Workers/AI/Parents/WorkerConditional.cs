using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Workers.AI.Parents
{
    public class WorkerConditional : Conditional
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