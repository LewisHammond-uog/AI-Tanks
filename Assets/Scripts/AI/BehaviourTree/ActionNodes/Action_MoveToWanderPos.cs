
using AI.BehaviourTree.BaseTypes;

namespace AI.BehaviourTree.ActionNodes
{
    public class Action_MoveToWanderPos : Action_MoveToPosition
    {
        protected override NodeStatus Update_Internal()
        {
            //Set move to position to be the wander position
            Owner.TurretComponent.SetTurretLookTarget(Blackboard.randomWanderPos);
            moveToPos = Blackboard.randomWanderPos;
            return base.Update_Internal();
        }
    }
}
