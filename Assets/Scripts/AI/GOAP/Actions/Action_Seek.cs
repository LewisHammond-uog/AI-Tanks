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
                Blackboard.lastSeenEnemyPosition = (Vector3)Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition().Item1;
                Owner.RemoveBelief(validLKPState.key);
            }
            else
            {
                return false;
            }
            
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Move to LKP
            if (!Owner.MovementCompoent.SetDestination(Blackboard.lastSeenEnemyPosition, true))
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