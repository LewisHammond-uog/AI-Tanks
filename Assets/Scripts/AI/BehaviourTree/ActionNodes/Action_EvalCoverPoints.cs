using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

namespace AI.BehaviourTree.ActionNodes
{
    public class Action_EvalCoverPoints : ActionNode
    {
        //Layer mask to not check when seeing if a cover point is visible
        [Header("Collisions")]
        [SerializeField] private LayerMask collisionCheckIgnoreLayer;

        [Header("Evaluation Values")]
        [Tooltip("Raise the distance to this power when evaluating how much score to deduct")]
        [SerializeField] private float distancePower = 2f;

        [Tooltip("Amount of score to deduct if this cover point is visible")] 
        [SerializeField] private float visibleScoreDeduction = 350f;

        [Tooltip("The minimum score to allow a valid 'best' cover point")]
        [SerializeField] private float minValidCoverPointScore = 500f;
        
        //Score to deduct from when determining cover point
        private const float baseScore = 1000;
    
        protected override NodeStatus Update_Internal()
        {
            //Check if cover points on the blackboard are valid
            if (Blackboard.validCoverPoints == null || Blackboard.validCoverPoints.Count == 0)
            {
                return NodeStatus.Fail;
            }
        
        
            //Store the best cover point so far
            CoverPoint bestCoverPoint = null;
            float bestCoverPointScore = Mathf.NegativeInfinity;
            foreach (CoverPoint coverPoint in Blackboard.validCoverPoints)
            {
                float score = ScoreCoverPoint(coverPoint);
                if (score > bestCoverPointScore)
                {
                    bestCoverPoint = coverPoint;
                    bestCoverPointScore = score;
                }
            }
        
            //Check we have a cover point over the minimum threshold so that we can't
            //choose a stupid cover point
            if (bestCoverPointScore >= minValidCoverPointScore)
            {
                Blackboard.bestCoverPoint = bestCoverPoint;
                return NodeStatus.Success;
            }
            else
            {
                Blackboard.bestCoverPoint = null;
                return NodeStatus.Fail;
            }
        }

        /// <summary>
        /// Score a cover point based on it's characterists 
        /// </summary>
        /// <param name="coverPoint">Point to score</param>
        /// <returns>Score of given cover point</returns>
        private float ScoreCoverPoint(CoverPoint coverPoint)
        {
            float totalScore = baseScore;
        
            //If the cover point is not valid then we cannot score it
            if (coverPoint == null)
            {
                return float.NegativeInfinity;
            }
        
            //Decrease score by distance squared
            totalScore -= Mathf.Pow(Vector3.Distance(Owner.transform.position, coverPoint.transform.position), distancePower);
        
            //If the cover can be see then decrease score because it likley can be seen by the enemy
            if (!Physics.Linecast(Owner.transform.position, coverPoint.transform.position, ~collisionCheckIgnoreLayer))
            {
                totalScore -= visibleScoreDeduction;
            }
        
            return totalScore;
        }
    }
}
