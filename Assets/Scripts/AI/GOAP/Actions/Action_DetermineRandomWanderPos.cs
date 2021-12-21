using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_DetermineRandomWanderPos : Action
    {

        [Header("State")] 
        //State for if we have a wander pos so that we can apply it to the player
        [SerializeField] private State hasWanderPosState;
        [SerializeField] private float wanderPosExpireTime = 10f;
        
        [Header("Wander")]
        //Radius to wander out to 
        [SerializeField] private float wanderRadius = 10f;
        
        protected override bool PrePerform()
        {
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Choose a random position on the nav mesh
            if (Owner.GetRandomPointOnNavMesh(out Vector3 randomPos, wanderRadius))
            {
                AgentBlackboard.randomWanderPos = randomPos;
                Owner.AddBelief(hasWanderPosState);
                return ActionState.Success;
            }

            return ActionState.Fail;
        }

        protected override bool PostPerform()
        {
            //Expire wander pos after time
            StartCoroutine(Owner.RemoveBeliefAfterTime(hasWanderPosState.key, wanderPosExpireTime));
            return true;
        }
    }
}