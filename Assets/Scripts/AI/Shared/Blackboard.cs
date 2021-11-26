using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [Header("Cover")]
    public List<CoverPoint> validCoverPoints; //List of cover points that this agent can go to
    public CoverPoint bestCoverPoint; //The best cover point that we have found

    [Header("Flee")] 
    public Vector3 fleeDirection; //Direction to run away from enemies close by

    [Header("Attack")] 
    public BaseAgent bestEnemyToAttack; //Best enemy that we can currently attack
    
    [Header("Seek")]
    public Vector3 lastSeenEnemyPosition; //Last position that we saw the enemy at - only updated after we can't see an enemy

    [Header("Wander")] 
    public CoverPoint targetCoverPoint; //Cover point that we are currently working towards
    public Vector3 randomWanderPos; //Random position on the navmesh to wander to
}
