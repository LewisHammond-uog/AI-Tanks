using AI.BehaviourTrees.BaseTypes;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Get a random position on the navmesh to wander to
/// </summary>
public class Action_GetRandomWanderPosition : ActionNode
{
    //Radius to wander out to 
    [SerializeField] private float wanderRadius = 10f;
    
    protected override NodeStatus Update_Internal()
    {
        if (Owner.GetRandomPointOnNavMesh(out Blackboard.randomWanderPos, wanderRadius))
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Fail;
    }
}
