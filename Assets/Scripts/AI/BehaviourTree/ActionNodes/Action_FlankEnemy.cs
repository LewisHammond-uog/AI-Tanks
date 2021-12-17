using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

//Action to find the best position to flank the enemy
namespace AI.BehaviourTree.ActionNodes
{
    public class Action_FlankEnemy : ActionNode
    {
        [Tooltip("Distance to project backwards when trying to project the enemy back vector at a far distance")]
        [SerializeField] private float farProjectDistance = 10f;
    
        [Tooltip("Distance to project backwards when trying to project the enemy back vector at a close distance")]
        [SerializeField] private float closeProjectDistance = 10f;
        protected override NodeStatus Update_Internal()
        {
            //Check we have a valid enemy
            if (!Blackboard.bestEnemyToAttack)
            {
                return NodeStatus.Fail;
            }

            Transform enemyTransform = Blackboard.bestEnemyToAttack.TurretComponent.transform;
            Vector3 enemyPos = enemyTransform.position;
            Vector3 enemyBackwards = -enemyTransform.forward;

            if (Owner.MovementCompoent.SetDestination(enemyPos + (enemyBackwards * farProjectDistance), true))
            {
                return NodeStatus.Success;
            }else if (Owner.MovementCompoent.SetDestination(enemyPos + (enemyBackwards * closeProjectDistance), true))
            {
                return NodeStatus.Success;
            }
            else
            {
                return NodeStatus.Fail;
            }
        
        }
    }
}
