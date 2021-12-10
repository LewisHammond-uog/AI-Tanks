using System.Collections;
using System.Collections.Generic;
using AI.GOAP;
using AI.GOAP.Actions;
using UnityEngine;

public class Action_MoveToCenter : Action
{
    private bool set = false;
    
    protected override bool PrePerform()
    {
        return true;
    }

    protected override ActionState Perform_Internal()
    {
        if (Owner && !set)
        {
            if (!Owner.MovementCompoent.SetDestination(Vector3.zero, true))
            {
                return ActionState.Fail;
            }
            set = true;
        }

        return Owner.MovementCompoent.IsAtDestination() ? ActionState.Success : ActionState.Running;
    }

    protected override bool PostPerform()
    {
        return false;
    }
}
