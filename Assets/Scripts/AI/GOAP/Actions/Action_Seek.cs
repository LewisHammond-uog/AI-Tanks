using Sensors.Hearing;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public abstract class Action_Seek : Action
    {
        protected override ActionState Perform_Internal()
        {
            //Move to LKP
            if (!Owner.MovementCompoent.SetDestination(AgentBlackboard.investigatePosition, true))
            {
                return ActionState.Fail;
            }

            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}