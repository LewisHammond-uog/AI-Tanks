using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.BehaviourTrees.BaseTypes;
using Unity.Collections;
using UnityEngine;

//Find all of the visible cover points
public class Action_FindCoverPoints : ActionNode
{
    //Radius of cover points to find
    [SerializeField] private float radius = 10f;
    private List<CoverPoint> allCoverPoints;
    private List<CoverPoint> coverPointsInRange;

    protected override void OnEnterNode()
    {
        allCoverPoints = CoverPoint.GetAllLevelCoverPoints().ToList();
    }

    protected override NodeStatus Update_Internal()
    {
        coverPointsInRange = new List<CoverPoint>();
        //Loop all of the cover points and only store ones that are a given distance away
        foreach (CoverPoint point in allCoverPoints)
        {
            if (Vector3.Distance(Owner.transform.position, point.transform.position) <= radius)
            {
                coverPointsInRange.Add(point);
            }
        }
        
        //If we have a valid cover point then we have succeded
        if (coverPointsInRange.Count > 0)
        {
            Blackboard.validCoverPoints = coverPointsInRange;
            return NodeStatus.Success;
        }
        else
        {
            Blackboard.validCoverPoints = new List<CoverPoint>();
            return NodeStatus.Fail;
        }
    }
}
