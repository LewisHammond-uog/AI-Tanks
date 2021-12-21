using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_Fire : Action
    {
        //States that used for refrences of names
        [SerializeField] private State canFireState;
        [SerializeField] private State hasValidEnemyState;
        
        //Values for what range we can multiply the velocity by to add randomness to our firing
        [SerializeField] private float minVelocityInfluence = 0.85f;
        [SerializeField] private float maxVeolcityInfluence = 1.15f;
        
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
            Vector3 enemyVelocity = Vector3.zero;
            if (AgentBlackboard?.bestEnemyToAttack)
            {
                enemyPosition = AgentBlackboard.bestEnemyToAttack.transform.position;
                enemyVelocity = AgentBlackboard.bestEnemyToAttack.MovementCompoent.Velocity;
            }
            
            //Calculate with velocity (and some added randomness)
            float velocityInfluence = Random.Range(minVelocityInfluence, maxVeolcityInfluence);
            Vector3 fireAtPos = enemyPosition + (enemyVelocity * velocityInfluence);

            Owner.ShootingComponent.Fire(PhysicsHelpers.CalculateLaunchVelocity(fireAtPos, Owner.ShootingComponent.FireTransform));
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