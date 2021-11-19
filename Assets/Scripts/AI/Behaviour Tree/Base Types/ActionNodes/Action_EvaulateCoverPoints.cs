using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

public class Action_EvaluateCoverPoints : ActionNode
{
    //Layer mask to not check when seeing if a cover point is visible
    [Header("Collisions")]
    [SerializeField] private LayerMask collisionCheckIgnoreLayer;

    [Header("Evaluation Values")]
    [Tooltip("Raise the distance to this power when evaluating how much score to deduct")]
    [SerializeField] private float distancePower = 2f;

    [Tooltip("Amount of score to deduct if this cover point is visible")] 
    [SerializeField] private float visibleScoreDeduction = 350f;
    
    protected override NodeStatus Update_Internal()
    {
        //Check if cover points on the blackboard are valid
        if (Blackboard.validCoverPoints == null || Blackboard.validCoverPoints.Count > 0)
        {
            return NodeStatus.Fail;
        }
        
        //If there is only 1 cover point then it must be the best
        if (Blackboard.validCoverPoints.Count == 1)
        {
            //todo get best cover point
            return NodeStatus.Success;
        }
        
        
        //Store the best cover point so far
        CoverPoint bestCoverPoint = null;
        foreach (CoverPoint coverPoint in Blackboard.validCoverPoints)
        {
            ScoreCoverPoint(coverPoint);
        }
        //todo - save cover point on blackboard
        //todo - return success if there is a good cover point (min threshold?)
    }

    /// <summary>
    /// Score a cover point based on it's characterists 
    /// </summary>
    /// <param name="coverPoint">Point to score</param>
    /// <returns>Score of given cover point</returns>
    private float ScoreCoverPoint(CoverPoint coverPoint)
    {
        const float baseScore = 1000;
        float totalScore = baseScore;
        
        //If the cover point is not valid then we cannot score it
        if (coverPoint == null)
        {
            return float.NegativeInfinity;
        }
        
        //Decrease score by distance squared
        totalScore -= Mathf.Pow(Vector3.Distance(Owner.transform.position, coverPoint.transform.position), distancePower);
        
        //If the cover can be see then decrease score because it likley can be seen by the enemy
        if (Physics.Linecast(Owner.transform.position, coverPoint.transform.position, ~collisionCheckIgnoreLayer))
        {
            totalScore -= visibleScoreDeduction;
        }
        
        return totalScore;
    }
}
