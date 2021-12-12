﻿using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_Wander : Action
    {
        //State that we have a wander position, so that we can remove it after we have finished wandering
        [SerializeField] private State hasWanderPosState;

        //If we have set the player to move to random pos
        private bool hasSetPos;
        
        protected override bool PrePerform()
        {
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Set Destination if we don't already have it set
            if (Owner.MovementCompoent.Destination != Blackboard.randomWanderPos)
            {
                Owner.MovementCompoent.SetDestination(Blackboard.randomWanderPos, true);
            }

            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            if (Owner.MovementCompoent.IsAtDestination())
            {
                Owner.RemoveBelief(hasWanderPosState.key);
            }

            return true;
        }
    }
}