using System;
using System.Collections.Generic;
using AI.GOAP.Goals;
using UnityEngine;

namespace AI.GOAP.Agent
{
    public class GOAPTankAgent : GOAPAgent
    {
        [SerializeField] private List<Goal> agentGoals;
        [SerializeField] private List<State> startingState;
        
        protected override void Start()
        {
            //Set goals - using the list to decrease the priority
            for (int i = 0; i < agentGoals.Count; i++)
            {
                AddGoal(agentGoals[i], agentGoals.Count - i);
            }
            
            //Set States
            foreach (State state in startingState)
            {
                if (state == null)
                {
                    continue;
                }
                AddBelief(state);
            }
            
            base.Start();
        }
    }
}