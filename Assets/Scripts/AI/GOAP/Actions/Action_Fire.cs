using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_Fire : Action
    {
        //States that used for refrences of names
        [SerializeField] private State canFireState;
        [SerializeField] private State hasValidEnemyState;
        
        protected override bool PrePerform()
        {
            //Double check that we can fire
            if (!Owner.ShootingComponent.CheckIfFireAllowed())
            {
                Owner.RemoveBelief(canFireState.key);
                return false;
            }

            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Determine the enemy position default to a position 20 units away if we do not have one
            Vector3 enemyPosition = transform.forward * 20f;
            if (AgentBlackboard?.bestEnemyToAttack)
            {
                enemyPosition = AgentBlackboard.bestEnemyToAttack.transform.position;
            }


            Owner.ShootingComponent.Fire(PhysicsHelpers.CalculateLaunchVelocity(enemyPosition, Owner.ShootingComponent.FireTransform));
            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            //Set that we cannot fire
            if (!Owner.ShootingComponent.CheckIfFireAllowed())
            {
                Owner.ModifyBelief(canFireState.key, false);
            }

            return true;
        }
    }
}