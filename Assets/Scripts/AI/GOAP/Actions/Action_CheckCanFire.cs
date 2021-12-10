namespace AI.GOAP.Actions
{
    public class Action_CheckCanFire : Action
    {
        protected override bool PrePerform()
        {
            return true;
        }

        protected override ActionState Perform_Internal()
        {
            if (Owner.ShootingComponent && Owner.ShootingComponent.CheckIfFireAllowed())
            {
                return ActionState.Success;
            }

            return ActionState.Fail;
        }

        protected override bool PostPerform()
        {
            return true;
        }
    }
}