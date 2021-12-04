using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;
using UnityEngine.AI;

namespace AI.BehaviourTree.ActionNodes
{
    /// <summary>
    /// Get a random position on the navmesh to wander to
    /// </summary>
    public class Action_GetRandomWanderPosition : ActionNode
    {
        //Radius to wander out to 
        [SerializeField] private float wanderRadius = 10f;
    
        protected override NodeStatus Update_Internal()
        {
            if (GetRandomPointOnNavMesh(out Blackboard.randomWanderPos))
            {
                return NodeStatus.Success;
            }

            return NodeStatus.Fail;
        }

        /// <summary>
        /// Get a random point on the Navmesh
        /// </summary>
        /// <param name="randomPos">[OUT] Random Position on the NavMesh</param>
        /// <returns>If position was on the navmesh/returns>
        private bool GetRandomPointOnNavMesh(out Vector3 randomPos)
        {
            //Determine Random Point
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += Owner.transform.position;
        
            //See if it is on the navmesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                randomPos = hit.position;
                return true;
            }

            randomPos = Vector3.zero;
            return false;
        }
    }
}
