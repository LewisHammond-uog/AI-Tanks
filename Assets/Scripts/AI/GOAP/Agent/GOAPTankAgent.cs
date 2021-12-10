using System;
using System.Collections.Generic;
using AI.GOAP.Goals;
using UnityEngine;

namespace AI.GOAP.Agent
{
    public class GOAPTankAgent : GOAPAgent
    {
        [SerializeField] private List<Goal> agentGoals;

        private void Start()
        {
            AddGoal(agentGoals[0], int.MaxValue);
        }
    }
}