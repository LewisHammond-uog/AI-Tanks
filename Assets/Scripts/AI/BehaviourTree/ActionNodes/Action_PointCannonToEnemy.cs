using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

//Action to point the cannon towards the enemy
namespace AI.BehaviourTree.ActionNodes
{
    public class Action_PointCannonToEnemy : ActionNode
    {
        [SerializeField] private float aimTolerance = 2f;
        //Turret Movement component
        private TurretMovement turret;
        protected override void OnEnterNode()
        {
            base.OnEnterNode();
            turret = Owner.TurretComponent;
        }

        protected override NodeStatus Update_Internal()
        {
            //Set turret to look at enemy
            turret.SetTurretLookTarget(AgentBlackboard.bestEnemyToAttack.transform);
        
            //Check if we are within tolerance of looking at the enemy
            if (turret.GetAngleToTurretTarget() < aimTolerance)
            {
                return NodeStatus.Success;
            }
            else
            {
                return NodeStatus.Running;
            }
        }
    }
}
