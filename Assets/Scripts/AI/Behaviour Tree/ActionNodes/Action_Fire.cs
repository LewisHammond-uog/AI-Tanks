using AI;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

//Action to fire the tanks turret
public class Action_Fire : ActionNode
{
    
    protected override NodeStatus Update_Internal()
    {
        if (Blackboard.bestEnemyToAttack == null)
        {
            return NodeStatus.Fail;
        }

        Vector3 enemyPosition = Blackboard.bestEnemyToAttack.transform.position;
        
        //Fire
        Owner.ShootingComponent.Fire(PhysicsHelpers.CalculateLaunchVelocity(enemyPosition, Owner.ShootingComponent.FireTransform));
        
        return NodeStatus.Success;
    }

}
