using System.Collections.Generic;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTrees.ActionNodes
{
    public class Action_DetermineBestEnemy : ActionNode
    {
        protected override void OnEnterNode()
        {
            base.OnEnterNode();
            Blackboard.bestEnemyToAttack = null;
        }

        protected override NodeStatus Update_Internal()
        {
            BaseAgent bestEnemy = Owner.DetermineBestEnemyToAttack();
            
            //If we don't have a best agent (is null) then there were no agents on the other team, fail
            if (bestEnemy == null)
            {
                return NodeStatus.Fail;
            }
            
            //Update the blackboard
            Blackboard.bestEnemyToAttack = bestEnemy;

            return NodeStatus.Success;
        }
        
    }
}