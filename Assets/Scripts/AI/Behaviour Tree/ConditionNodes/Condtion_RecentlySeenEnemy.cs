using System;
using System.Linq;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;


//Condition for if we have recently seen an enemy
public class Condtion_RecentlySeenEnemy : ActionNode
{
    //The maximum time since we last saw an enemy to act on it
    [SerializeField] private float allowableTimeSinceSeen = 10f;
    
    protected override NodeStatus Update_Internal()
    {
        //Make sure that we have no visible agents - fail if we have one
        int visibleAgentCount = Owner.VisionKnowledgeComponent.GetVisibleAgents().Count();
        if (visibleAgentCount == 0)
        {
            return NodeStatus.Fail;
        }
        
        //Get the last seen agent position
        Tuple<Vector3?, float> lastSeenAgentPosTime = Owner.VisionKnowledgeComponent.GetLastSeenAgentPosition();
        Vector3? lastSeenPosition = lastSeenAgentPosTime.Item1;
        float lastSeenTime = lastSeenAgentPosTime.Item2;
        if (lastSeenPosition == null)
        {
            return NodeStatus.Fail;
        }
        
        //Check if the time is too long
        float timeSinceLastSeen = Time.timeSinceLevelLoad - lastSeenTime;
        if (timeSinceLastSeen > allowableTimeSinceSeen)
        {
            return NodeStatus.Fail;
        }

        return NodeStatus.Success;
    }
}