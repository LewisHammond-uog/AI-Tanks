using System.Collections;
using System.Collections.Generic;
using AI.GOAP;
using UnityEngine;

public class TestAction : Action
{
    protected override bool PrePerform()
    {
        return true;
    }

    protected override ActionState Perform_Internal()
    {
        return ActionState.Success;
    }

    protected override bool PostPerform()
    {
        return false;
    }
}
