using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_DetermineFleeDirection : Action
    {
        [Header("Reset")] 
        //Time after which we reset back to NOT having a flee position
        [SerializeField] private float resetTime = 1.0f;
        //State that is our effect of having a flee direction - so that we can remove it after 5 seconds
        [SerializeField] private State hasFleeDirState;
        
        protected override bool PrePerform()
        {
            //Check that we have a movement component and a enemy to flank
            if (!Owner.MovementCompoent)
            {
                return false;
            }

            AgentBlackboard.fleeDirection = Vector3.zero;
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Caculate a direction vector to be the average of all of the agents I can see,
            //then go the other way
            List<BaseAgent> visibleAgents = Owner.VisionKnowledgeComponent.GetVisibleAgents().ToList();
            if (visibleAgents.Count == 0)
            {
                return ActionState.Fail;
            }
        
            Vector3 sumVector = Vector3.zero;
            foreach (BaseAgent agent in visibleAgents)
            {
                //Add the direction
                sumVector += (agent.transform.position - Owner.transform.position).normalized;
            }
            //Get the average and normalize for a direction
            Vector3 agentAvgDirection = (sumVector / visibleAgents.Count).normalized;
        
            //Go the other direction!
            AgentBlackboard.fleeDirection = -agentAvgDirection;
            
            //Add Belief to agents
            Owner.AddBelief(hasFleeDirState);
            
            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            //Expire flee dir after time
            StartCoroutine(Owner.RemoveBeliefAfterTime(hasFleeDirState.key, resetTime));
            return true;
        }
    }
}
