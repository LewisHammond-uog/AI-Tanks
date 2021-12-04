using System.Collections.Generic;
using System.Linq;
using AI;
using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

public class Condition_CanSeeEnemy : ActionNode
{
    [Range(0,1)] [Tooltip("Ammount (0-1) that we can see an agent before considering it visible")]
    [SerializeField] private float visionThreshold = 0.1f;
    
    protected override NodeStatus Update_Internal()
    {
        //Are there any enemies within our vision cones that are NOT on our team
        List<BaseAgent> visibleAgents = Owner.VisionKnowledgeComponent.GetVisibleAgents(visionThreshold).ToList();
        
        //Any of the agents are NOT on our team then we can see an enemy
        return visibleAgents.Any(agent => agent.Team != Owner.Team) ? NodeStatus.Success : NodeStatus.Fail;
    }
}
