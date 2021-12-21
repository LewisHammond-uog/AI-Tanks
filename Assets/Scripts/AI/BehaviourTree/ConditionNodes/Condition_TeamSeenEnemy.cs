using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using AI.Shared.Blackboard;
using UnityEngine;

namespace AI.BehaviourTrees.ConditionNodes
{
    /// <summary>
    /// Condition to check if the team blackboard has a valid enemy
    /// </summary>
    public class Condition_TeamSeenEnemy : ActionNode
    {
        //Team Blackboard
        private TeamBlackboard teamBlackboard;
        
        //The maximum time since we last saw an enemy to act on it
        [SerializeField] private float allowableTimeSinceSeen = 10f;

        protected override void OnEnterNode()
        {
            teamBlackboard = TeamBlackboardManager.GetBlackboard(Owner.Team);
        }

        protected override NodeStatus Update_Internal()
        {
            TeamBlackboard.Entry blackboardEntry;
            
            //Check if an agent can see an enemy or if a team mate has a recently seen enemy
            if(!teamBlackboard.TryGetEntry(VisionKnowledge.SeenAgentKey, allowableTimeSinceSeen, out blackboardEntry))
            {
                return NodeStatus.Fail;
            }else if (blackboardEntry == null && 
                  !teamBlackboard.TryGetEntry(VisionKnowledge.LastKnownPosKey, allowableTimeSinceSeen, out blackboardEntry))
            {
                return NodeStatus.Fail;
            }
            
            //Check we have a valid blackboard entry
            if (blackboardEntry == null)
            {
                return NodeStatus.Fail;
            }
            
            //Update our investigate position
            AgentBlackboard.investigatePosition = blackboardEntry.Value;
            return NodeStatus.Success;
        }
    }
}