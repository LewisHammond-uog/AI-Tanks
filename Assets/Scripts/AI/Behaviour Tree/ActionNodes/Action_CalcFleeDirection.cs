using System.Collections.Generic;
using System.Linq;
using AI;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

public class Action_CalcFleeDirection : ActionNode
{
    protected override NodeStatus Update_Internal()
    {
        //Caculate a direction vector to be the average of all of the agents I can see,
        //then go the other way
        //Get a list of all the agents that I can see
        List<BaseAgent> visibleAgents = Owner.VisionKnowledgeComponent.GetVisibleAgents().ToList();
        if (visibleAgents.Count == 0)
        {
            return NodeStatus.Fail;
        }
        
        Vector3 sumVector = Vector3.zero;
        foreach (BaseAgent agent in visibleAgents)
        {
            //Add the direction
            sumVector += (agent.transform.position - Owner.transform.position).normalized;
        }
        //Get the average and normalize for a direction
        Vector3 agentAvgDirection = (sumVector / visibleAgents.Count).normalized;
        
        //Go the other direction!
        Blackboard.fleeDirection = -agentAvgDirection;
        return NodeStatus.Success;
    }
}
