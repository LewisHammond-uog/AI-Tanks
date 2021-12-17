using Sensors.Hearing;
using UnityEngine;

namespace AI.GOAP.Actions
{
    //Extension of the Seek class for vision
    class Action_SeekVision : Action_Seek
    {
        //Valid LKP state that we can remove if the LKP is NOT valid
        [SerializeField] private State validLKPState;
        
        protected override bool PrePerform()
        {
            //Update seek position based on last seen
            Vector3? lastSeenAgent = Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition().Item1;
            if (Owner.VisionKnowledgeComponent && lastSeenAgent != null)
            {
                Blackboard.investigatePosition = (Vector3)Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition().Item1;
            }else
            {
                Owner.RemoveBelief(validLKPState.key);
                return false;
            }

            //Set Turret to look at LKP
            Owner.TurretComponent.SetTurretLookTarget(Blackboard.investigatePosition);
            
            return true;

        }
    }
}