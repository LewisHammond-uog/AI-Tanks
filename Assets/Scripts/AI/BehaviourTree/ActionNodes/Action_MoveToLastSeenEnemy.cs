
using AI.BehaviourTree.BaseTypes;

namespace AI.BehaviourTree.ActionNodes
{
    /// <summary>
    /// Node to move to the last seen enemy position
    /// </summary>
    public class Action_MoveToLastSeenEnemy : Action_MoveToPosition
    {
        protected override NodeStatus Update_Internal()
        {
            //Set the move to position and then call base movement
            moveToPos = Blackboard.lastSeenEnemyPosition;
            return base.Update_Internal();
        }
    }
}
