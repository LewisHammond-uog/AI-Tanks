using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_FlankEnemy : Action
    {
        [Header("States")]
        //Remove this state if it turns out that we don't have a flank position
        [SerializeField] private State hasFlankPosState;

        protected override bool PrePerform()
        {
            //Check that we have a flank position
            if (Blackboard.flankPosition == null)
            {
                Owner.RemoveBelief(hasFlankPosState.key);
                return false;
            }

            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Move to flank position
            if (Blackboard.flankPosition != null &&
                Owner.MovementCompoent.SetDestination((Vector3) Blackboard.flankPosition, true))
            {
                return ActionState.Success;
            }

            return ActionState.Fail;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}