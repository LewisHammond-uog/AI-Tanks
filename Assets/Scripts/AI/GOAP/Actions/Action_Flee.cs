using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_Flee : Action
    {
        
        [Tooltip("Distance to project the flee direction when attemping to project it far away")]
        [SerializeField] private float farProjectDistance = 10f;
        
        [Tooltip("Distance to project the flee direction when attemping to project it close")]
        [SerializeField] private float closeProjectDistance = 2f;
        
        //Check for if we can shoot to abort this state
        [SerializeField] private State canFireState;
        
        //Have we already set a position on this node runthrough?
        private bool positionSet;
        
        protected override bool PrePerform()
        {
            //Check that we have a valid direction
            if (Blackboard?.fleeDirection == Vector3.zero)
            {
                return false;
            }

            positionSet = false;
            
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Check that we cannot fire, if we can then we should abort this plan
            if (Owner.HasBelief(canFireState))
            {
                return ActionState.Fail;
            }
            
            
            if (positionSet == false)
            {
                //Project the flee direction, see if we can go far, then less far
                if (Owner.MovementCompoent.SetDestination(Blackboard.fleeDirection * farProjectDistance, true))
                {
                    positionSet = true;
                    return ActionState.Running;
                }
                else if (Owner.MovementCompoent.SetDestination(Blackboard.fleeDirection * closeProjectDistance, true))
                {
                    positionSet = true;
                    return ActionState.Running;
                }
                else
                {
                    //We cannot project less than our shortest flee distance we therefore cannot run away!
                    return ActionState.Fail;
                }
            }
            
            //Check if we are at destination
            const float distanceThreshold = 0.5f;
            return Owner.MovementCompoent.IsAtDestination(distanceThreshold) ? ActionState.Success : ActionState.Running;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}