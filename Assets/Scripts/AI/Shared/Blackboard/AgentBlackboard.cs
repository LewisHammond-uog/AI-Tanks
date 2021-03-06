using System.Collections.Generic;
using UnityEngine;

namespace AI.Shared.Blackboard
{
    [System.Serializable]
    public class AgentBlackboard
    {
        [Header("Cover")]
        public List<CoverPoint> validCoverPoints; //List of cover points that this agent can go to
        public CoverPoint bestCoverPoint; //The best cover point that we have found

        [Header("Flee")] 
        public Vector3 fleeDirection; //Direction to run away from enemies close by

        [Header("Attack")] 
        public BaseAgent bestEnemyToAttack; //Best enemy that we can currently attack
        public Vector3? flankPosition; //Position to flank to (only used by GOAP)
    
        [Header("Seek")]
        public Vector3 investigatePosition; //Last position that we saw the enemy at - only updated after we can't see an enemy

        [Header("Wander")] 
        public CoverPoint targetCoverPoint; //Cover point that we are currently working towards
        public Vector3 randomWanderPos; //Random position on the navmesh to wander to
    }
}
