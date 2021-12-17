using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_Seek : Action
    {
        //Valid LKP state that we can remove if the LKP is NOT valid
        [SerializeField] private State validLKPState;
        
        protected override bool PrePerform()
        {
            
            //Update seek position based on last seen
            if (Owner.VisionKnowledgeComponent && Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition().Item1 != null)
            {
                Blackboard.investigatePosition = (Vector3)Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition().Item1;
            }
            else
            {
                Owner.RemoveBelief(validLKPState.key);
                return false;
            }
            
            //Set Turret to look at LKP
            Owner.TurretComponent.SetTurretLookTarget(Blackboard.investigatePosition);
            
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Move to LKP
            if (!Owner.MovementCompoent.SetDestination(Blackboard.investigatePosition, true))
            {
                return ActionState.Fail;
            }

            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}