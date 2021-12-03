
using AI.BehaviourTrees.BaseTypes;

public class Action_MoveToWanderPos : Action_MoveToPosition
{
    protected override NodeStatus Update_Internal()
    {
        //Set move to position to be the wander position
        moveToPos = Blackboard.randomWanderPos;
        return base.Update_Internal();
    }
}
