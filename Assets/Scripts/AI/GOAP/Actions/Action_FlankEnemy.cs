using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_FlankEnemy : Action
    {
        [Header("States")]
        //Remove this state if it turns out that we don't have a flank position
        [SerializeField] private State hasFlankPosState;
        //Check for if we can shoot to abort this state
        [SerializeField] private State canFireState;

        private bool hasSetFlankPos;

        protected override bool PrePerform()
        {
            hasSetFlankPos = false;
            
            //Check that we have a flank position
            if (AgentBlackboard.flankPosition == null)
            {
                Owner.RemoveBelief(hasFlankPosState.key);
                return false;
            }
            
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Check that we cannot fire, if we can then we should abort this plan
            if (Owner.HasBelief(canFireState))
            {
                return ActionState.Fail;
            }
            
            
            //Move to flank position if we haven't set it already
            if (!hasSetFlankPos)
            {
                if (AgentBlackboard.flankPosition != null)
                    Owner.MovementCompoent.SetDestination((Vector3) AgentBlackboard.flankPosition, true);
                hasSetFlankPos = true;
            }
            
            //Set to look at enemy
            if (AgentBlackboard.bestEnemyToAttack)
            {
                Owner.TurretComponent.SetTurretLookTarget(AgentBlackboard.bestEnemyToAttack.transform);
            }

            //If we are at the destination then return success otherwise running
            if (Owner.MovementCompoent.IsAtDestination())
            {
                return ActionState.Success;
            }
            else
            {
                return ActionState.Running;
            }
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}