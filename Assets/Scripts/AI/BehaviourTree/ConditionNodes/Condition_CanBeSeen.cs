using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AI;
using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;

//Condition to Check if this agent can be seen
public class Condition_CanBeSeen : ActionNode
{
    private VisionKnowledge vision;

    [Tooltip("Layers to exclude when check if this agent can be seen")]
    [SerializeField] private LayerMask checkExcludeLayer;

    protected override void OnEnterNode()
    {
        vision = Owner.VisionKnowledgeComponent;
    }

    protected override NodeStatus Update_Internal()
    {
        //Get a list of all the agents that I can see
        List<BaseAgent> visibleAgents = vision.GetVisibleAgents().ToList();

        //No visible agents - then I can't be seen by anything that I can see
        if(visibleAgents.Count == 0)
        {
            return NodeStatus.Fail;
        }

        //Do line casts from all of the agents that I can see - excluding agents, can I be seen by any of them? 
        foreach(BaseAgent agent in visibleAgents)
        {
            bool canBeSeen = !Physics.Linecast(Owner.transform.position, agent.transform.position, ~checkExcludeLayer);
            if (canBeSeen)
            {
                return NodeStatus.Success;
            }
        }

        return NodeStatus.Fail;
    }
}
