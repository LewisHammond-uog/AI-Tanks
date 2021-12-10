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
            //Get enemy to attack
            if (Blackboard.bestEnemyToAttack == null)
            {
                //Remove valid enemy state
                Owner.RemoveBelief(hasValidEnemyState.key);
                return ActionState.Fail;
            }

            Vector3 enemyPosition = Blackboard.bestEnemyToAttack.transform.position;
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