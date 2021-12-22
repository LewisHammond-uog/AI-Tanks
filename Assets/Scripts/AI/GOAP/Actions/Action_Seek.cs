using Sensors.Hearing;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public abstract class Action_Seek : Action
    {
        //Interupt if we can see an enemy
        [SerializeField] private State CanSeeEnemyState;
        
        protected override bool PrePerform()
        {
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
            }
            
            
            return true;
        }
    }
}