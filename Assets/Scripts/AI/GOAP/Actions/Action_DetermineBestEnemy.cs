using System.Collections.Generic;
using UnityEngine;

//Action to find the best enemy to fire at
namespace AI.GOAP.Actions
{
    public class Action_DetermineBestEnemy : Action
    {
        protected override bool PrePerform()
        {
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            BaseAgent bestEnemy = Owner.DetermineBestEnemyToAttack();
            
            //Update the blackboard
            Blackboard.bestEnemyToAttack = bestEnemy;
            
            //If we don't have a best agent (is null) then there were no agents on the other team, fail
            return bestEnemy == null ? ActionState.Fail : ActionState.Success;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}
