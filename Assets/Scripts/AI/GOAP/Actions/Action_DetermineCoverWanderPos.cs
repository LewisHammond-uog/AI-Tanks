using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP.Actions
{
//Action to determine a wander position that is the closest cover position
    public class Action_DetermineCoverWanderPos : Action
    {
        
        [Header("State")] 
        //State for if we have a wander pos so that we can apply it to the player
        [SerializeField] private State hasWanderPosState;
        [SerializeField] private float wanderPosExpireTime = 30f;

        private List<CoverPoint> allCoverPoints;
        
        protected override bool PrePerform()
        {
            //Check that we have valid cover points
            allCoverPoints = CoverPoint.GetAllLevelCoverPoints();
            return allCoverPoints != null && allCoverPoints.Count != 0;
        }

        protected override ActionState Perform_Internal()
        {
            //Find the closest cover point
            CoverPoint closestCoverPoint = FindClosestCoverPoint(allCoverPoints);
            if (closestCoverPoint == null)
            {
                return ActionState.Fail;
            }
            
            //Set the cover point as the wander position and set owner knowing that it has a wander position
            Blackboard.randomWanderPos = closestCoverPoint.transform.position;
            Owner.AddBelief(hasWanderPosState);
            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            //Expire wander pos after time
            StartCoroutine(Owner.RemoveBeliefAfterTime(hasWanderPosState.key, wanderPosExpireTime));
            return true;
        }

        /// <summary>
        /// Find the cover point
        /// </summary>
        /// <param name="coverPoints"></param>
        private CoverPoint FindClosestCoverPoint(List<CoverPoint> coverPoints)
        {
            //Store closest cover point as a tuple
            Tuple<CoverPoint, float> bestCoverPointDistance = new Tuple<CoverPoint, float>(null, float.PositiveInfinity);
            foreach (CoverPoint coverPoint in coverPoints)
            {
                float distance = Vector3.Distance(Owner.transform.position, coverPoint.transform.position);
                if (distance < bestCoverPointDistance.Item2)
                {
                    bestCoverPointDistance = new Tuple<CoverPoint, float>(coverPoint, distance);
                }
            }
            return bestCoverPointDistance.Item1;
        }
    }
}
