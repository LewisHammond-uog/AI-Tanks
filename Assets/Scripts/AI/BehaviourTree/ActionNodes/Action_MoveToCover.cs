
using AI.BehaviourTree.BaseTypes;

namespace AI.BehaviourTree.ActionNodes
{
    public class Action_MoveToCover : Action_MoveToPosition
    {
        protected override NodeStatus Update_Internal()
        {
            //Get the best cover position and set that as where we want to move to
            //allow Action_MoveToPosition to handle the rest
            moveToPos = Blackboard.bestCoverPoint.transform.position;
            return base.Update_Internal();
        }
    }
}
