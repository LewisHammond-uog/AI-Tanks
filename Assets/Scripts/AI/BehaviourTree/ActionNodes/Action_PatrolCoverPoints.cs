using System;
using System.Collections.Generic;
using System.Linq;
using AI.BehaviourTree.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTree.ActionNodes
{
    /// <summary>
    /// Node to patrol the list of valid cover points
    /// </summary>
    public class Action_PatrolCoverPoints : Action_MoveToPosition
    {
        private LinkedList<CoverPoint> patrolRoute;
        private LinkedListNode<CoverPoint> currentPatrolNode; //Current node in the linked list

        //Position of the current patrol node
        private Vector3? currentPatrolTargetPosition => currentPatrolNode.Value.transform.position;

        protected override void OnEnterNode()
        {
            //Build a distance sorted list of the cover points
            List<Tuple<CoverPoint, float>> coverPointDistanceList =
                new List<Tuple<CoverPoint, float>>(AgentBlackboard.validCoverPoints.Count);

            //Calculate the distances for each cover point
            foreach (CoverPoint coverPoint in AgentBlackboard.validCoverPoints)
            {
                float distance = Vector3.Distance(Owner.transform.position, coverPoint.transform.position);
                coverPointDistanceList.Add(new Tuple<CoverPoint, float>(coverPoint, distance));
            }
        
            //Sort the list by the distance value
            coverPointDistanceList.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        
            //Covert to a linked list of cover points to form the patrol route
            patrolRoute = new LinkedList<CoverPoint>(coverPointDistanceList.Select(_ => _.Item1).ToList());
        
            //Set the current node to be the first in the list
            if (patrolRoute.Count > 0)
            {
                currentPatrolNode = patrolRoute.First;
            }
        }

        protected override NodeStatus Update_Internal()
        {
            //Fail if we don't have a valid patrol route or node to move to
            if (patrolRoute == null || patrolRoute.Count == 0 || currentPatrolNode == null)
            {
                return NodeStatus.Fail;
            }
        
            //Check if we are have a destination
            if (moveToPos == null || (moveToPos != null && moveToPos != currentPatrolTargetPosition ))
            {
                moveToPos = (Vector3)currentPatrolTargetPosition;
            }
            
            //Do Movement
            NodeStatus movementResult = base.Update_Internal();
            Owner.TurretComponent.SetTurretLookTarget((Vector3)moveToPos);
        
            //Check if we are at the destination
            if (Owner.MovementCompoent.IsAtDestination())
            {
                //Move to the next node in the list
                currentPatrolNode = currentPatrolNode.Next;
            
                //Check if we have finished the patrol
                return currentPatrolNode == null ? NodeStatus.Success : NodeStatus.Running;
            }

            return movementResult;
        }

        protected override void OnExitNode()
        {
            patrolRoute = null;
            moveToPos = null;
            base.OnExitNode();
        }
    }
}
