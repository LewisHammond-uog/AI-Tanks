using Sensors.Hearing;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public abstract class Action_Seek : Action
    {
        //Is our destintation set?
        private bool isDestSet = true;

        //Interupt if we can see an enemy
        [SerializeField] private State CanSeeEnemyState;
        
        protected override bool PrePerform()
        {
            isDestSet = false;
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Interrupt if we seen an enemy
            if (Owner.HasBelief(CanSeeEnemyState))
            {
                return ActionState.Fail;
            }
            
            //Move to LKP
            if (Owner.MovementCompoent.Destination != AgentBlackboard.investigatePosition)
            {
                if (!Owner.MovementCompoent.SetDestination(AgentBlackboard.investigatePosition, true))
                {
                    return ActionState.Fail;
                }
                isDestSet = true;
            }
            
            //If we are not at the destination then we are running
            if (!Owner.MovementCompoent.IsAtDestination())
            {
                Owner.TurretComponent.SetTurretLookTarget(AgentBlackboard.investigatePosition);
                return ActionState.Running;
            }

            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            //If we are at destination then invalidate our LKP
            if (Owner.MovementCompoent.IsAtDestination())
            {
                Owner.VisionKnowledgeComponent.InvalidateLKP();
                Debug.Log("Is at Dest, Invalidating LKP");
            }
            
            
            return true;
        }
    }
}