using Sensors.Hearing;
using UnityEngine;

namespace AI.GOAP.Actions
{
    //Extension of the Seek class for hearing
    class Action_SeekHearing : Action_Seek
    {
        //Valid Sound state that we can remove if the LKP is NOT valid
        [SerializeField] private State validSoundState;
        
        protected override bool PrePerform()
        {
            //Update seek position based on last heard
            SoundDrop mostHeardSound = Owner.HearingKnowledgeComponent.GetMostHeardSound();
            if (Owner.HearingKnowledgeComponent && mostHeardSound != null)
            {
                AgentBlackboard.investigatePosition = mostHeardSound.transform.position;
            }else
            {
                Owner.RemoveBelief(validSoundState.key);
                return false;
            }
            
            //Set Turret to look at LKP
            Owner.TurretComponent.SetTurretLookTarget(AgentBlackboard.investigatePosition);
            return true;

        }
    }
}