﻿using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Assistant.AI.Parents
{
    public class CargoAssistantAction : Action
    {
        protected CargoAssistant CargoAssistant;
        protected NavMeshAgent Agent;

        public override void OnAwake()
        {
            CargoAssistant ??= GetComponent<CargoAssistant>();
            Agent ??= GetComponent<NavMeshAgent>();
        }
    }
}