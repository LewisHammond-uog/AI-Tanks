using UnityEngine;

namespace AI.GOAP.Actions
{
    public class Action_PointCannonToEnemy : Action
    {
        //Allowed offset angle
        [SerializeField] private float aimTolerance = 2f;
        
        //Have be set to rotate the turret?
        private bool setToRotate = false;

        //Turret Movement component
        private TurretMovement turret;
    
        protected override bool PrePerform()
        {
            //If we don't have an enemy to attack then fail
            if (!Blackboard?.bestEnemyToAttack)
            {
                return false;
            }

            //Check we have a turret
            if (!Owner.TurretComponent)
            {
                return false;
            }
            turret = Owner.TurretComponent;

            setToRotate = false;

            return true;
        }

        protected override ActionState Perform_Internal()
        {
            //Set to rotate the turret towards the enemy
            if (!setToRotate)
            {
                Owner.TurretComponent.TurretLookAtPoint = Blackboard.bestEnemyToAttack.transform.position;
                setToRotate = true;
            }
        
            //Rotate the turret
            if (turret.GetAngleToTurretTarget() < aimTolerance)
            {
                return ActionState.Success;
            }
            else
            {
                return ActionState.Running;
            }
        }

        protected override bool PostPerform()
        {
            setToRotate = false;
            return true;
        }
    }
}
