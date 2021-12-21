using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_DetermineFlankPos : Action
    {
        
        [Header("Position Finding")]
        [Tooltip("Distance to project backwards when trying to project the enemy back vector at a far distance")]
        [SerializeField] private float farProjectDistance = 10f;
    
        [Tooltip("Distance to project backwards when trying to project the enemy back vector at a close distance")]
        [SerializeField] private float closeProjectDistance = 2f;

        [Header("Reset")] 
        //Time after which we reset back to NOT having a flank position
        [SerializeField] private float resetTime = 1.0f;
        //State that is our effect of having a flank position - so that we can remove it after 5 seconds
        [SerializeField] private State hasFlankPosState;
        
        protected override bool PrePerform()
        {
            //Check that we have a movement component and a enemy to flank
            if (!Owner.MovementCompoent || !AgentBlackboard?.bestEnemyToAttack)
            {
                return false;
            }

            AgentBlackboard.flankPosition = null;

            return true;
        }

        protected override ActionState Perform_Internal()
        {
            Transform enemyTransform = AgentBlackboard.bestEnemyToAttack.TurretComponent.transform;
            Vector3 enemyPos = enemyTransform.position;
            Vector3 enemyBackwards = -enemyTransform.forward;

            Vector3 closeProjectPos = enemyPos + (enemyBackwards * closeProjectDistance);
            Vector3 farProjectPos = enemyPos + (enemyBackwards * farProjectDistance);
            
            //Try and set navmesh destination then add Belief to the owner that they have a flank position
            if (Owner.MovementCompoent.SetDestination(farProjectPos))
            {
                Owner.AddBelief(hasFlankPosState);
                AgentBlackboard.flankPosition = closeProjectPos;
                return ActionState.Success;
            }else if (Owner.MovementCompoent.SetDestination(closeProjectPos))
            {
                Owner.AddBelief(hasFlankPosState);
                AgentBlackboard.flankPosition = enemyPos + (enemyBackwards * farProjectDistance);
                return ActionState.Success;
            }
            else
            {
                return ActionState.Fail;
            }
        }

        protected override bool PostPerform()
        {
            //Remove belief from agent after a number of seconds
            StartCoroutine(Owner.RemoveBeliefAfterTime(hasFlankPosState.key, resetTime));
            return true;
        }
    }
}