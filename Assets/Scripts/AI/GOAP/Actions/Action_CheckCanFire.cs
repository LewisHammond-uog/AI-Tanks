using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_CheckCanFire : Action
    {
        [Header("Agent Effects")]
        [SerializeField] private State hasCheckedFireState;
        [SerializeField] private State canFireState;
        
        [Header("Expiry")]
        [SerializeField] private bool expires = true;
        [SerializeField] private float checkExpireTime = 20.0f;
        
        protected override bool PrePerform()
        {
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            if (!Owner.ShootingComponent || !Owner.ShootingComponent.CheckIfFireAllowed())
            {
                return ActionState.Fail;
            }
            
            //If we can fire then set the agent belief 
            Owner.AddBelief(canFireState);
            return ActionState.Success;
        }

        protected override bool PostPerform()
        {
            Owner.AddBelief(hasCheckedFireState);
            //Remove belief after time
            if (expires)
            {
                StartCoroutine(Owner.RemoveBeliefAfterTime(hasCheckedFireState.key, checkExpireTime));
            }
            return true;
        }
    }
}