using AI.Shared.Blackboard;
using UnityEngine;

namespace AI.GOAP.Actions
{
    /// <summary>
    /// Action to seek based off team knowledge
    /// </summary>
    class Action_SeekTeamKnowledge : Action_Seek
    {
        //Valid Team Info state that we can remove if the Team Info is NOT valid
        [SerializeField] private State validTeamInfoState;
        
        protected override bool PrePerform()
        {
            TeamBlackboard teamBlackboard = TeamBlackboardManager.GetBlackboard(Owner.Team);

            //Get the blackboard entry out of either the currently seen agent or the last known position
            TeamBlackboard.Entry blackboardEntry;
            teamBlackboard.TryGetEntry(VisionKnowledge.SeenAgentKey, out blackboardEntry);
            if (blackboardEntry == null)
            {
                teamBlackboard.TryGetEntry(VisionKnowledge.LastKnownPosKey, out blackboardEntry);
            }
            
            //If we don't get a valid entry out of either then we have an invalid team info state
            if (blackboardEntry == null)
            {
                Owner.RemoveBelief(validTeamInfoState.key);
                return false;
            }
            
            //Set our investigate position
            AgentBlackboard.investigatePosition = blackboardEntry.Value;
            
            //Set Turret to at investigate
            Owner.TurretComponent.SetTurretLookTarget(AgentBlackboard.investigatePosition);
            
            return true;
        }
    }
}