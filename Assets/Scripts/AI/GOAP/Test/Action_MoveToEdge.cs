using System.Collections;
using System.Collections.Generic;
using AI.GOAP;
using AI.GOAP.Actions;
using UnityEngine;

public class Action_MoveToEdge : Action
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
            if (!Owner.MovementCompoent.SetDestination(new Vector3(-6.8f, 0, -36.8f), true))
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
