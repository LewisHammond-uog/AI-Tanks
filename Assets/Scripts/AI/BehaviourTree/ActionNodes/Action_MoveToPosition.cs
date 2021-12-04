using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

namespace AI.BehaviourTree.ActionNodes
{
    public abstract class Action_MoveToPosition : ActionNode
    {
        //Position to move to - null if none has been set
        protected Vector3? moveToPos;

        //If we should start movement if a valid route is found
        [SerializeField] protected bool startMovementOnRouteFound = true;

        protected override void OnEnterNode()
        {
            moveToPos = null;
        }

        protected override NodeStatus Update_Internal()
        {
            //If there is no move to position set then we should fail as we cannot set
            //the nav mesh to move us anywhere
            if (moveToPos == null)
            {
                return NodeStatus.Fail;
            }

            //Set Destination if we don't already have it set
            if (Owner.MovementCompoent.Destination != moveToPos)
            {
                Owner.MovementCompoent.SetDestination((Vector3) moveToPos, startMovementOnRouteFound);
            }
        
            //Check if we are at the destination, if we are then we succeded otherwise we are still running
            return Owner.MovementCompoent.IsAtDestination() ? NodeStatus.Success : NodeStatus.Running;
        }
    }
}
