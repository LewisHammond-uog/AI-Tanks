using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [Header("Cover")]
    public List<CoverPoint> validCoverPoints; //List of cover points that this agent can go to
    public CoverPoint bestCoverPoint; //The best cover point that we have found

    [Header("Flee")] 
    public Vector3 fleeDirection;
}
