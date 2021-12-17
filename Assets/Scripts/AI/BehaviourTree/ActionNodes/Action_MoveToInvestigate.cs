
using AI.BehaviourTree.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTree.ActionNodes
{
    /// <summary>
    /// Node to move to the last seen enemy position
    /// </summary>
    public class Action_MoveToInvestigate : Action_MoveToPosition
    {
        protected override void OnEnterNode()
        {
            base.OnEnterNode();
            Owner.TurretComponent.SetTurretLookTarget(Blackboard.investigatePosition);
        }

        protected override NodeStatus Update_Internal()
        {
            //Set the move to position and then call base movement
            moveToPos = Blackboard.investigatePosition;
            return base.Update_Internal();
        }
    }
}
