using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

//Action to fire the tanks turret
namespace AI.BehaviourTree.ActionNodes
{
    public class Action_Fire : ActionNode
    {
        //Values for what range we can multiply the velocity by to add randomness to our firing
        [SerializeField] private float minVelocityInfluence = 0.85f;
        [SerializeField] private float maxVeolcityInfluence = 1.15f;
        
        protected override NodeStatus Update_Internal()
        {
            if (AgentBlackboard.bestEnemyToAttack == null)
            {
                return NodeStatus.Fail;
            }

            Vector3 enemyPosition = AgentBlackboard.bestEnemyToAttack.transform.position;
            Vector3 enemyVelocity = AgentBlackboard.bestEnemyToAttack.MovementCompoent.Velocity;
            
            //Calculate with velocity (and some added randomness)
            float velocityInfluence = Random.Range(minVelocityInfluence, maxVeolcityInfluence);
            Vector3 fireAtPos = enemyPosition + (enemyVelocity * velocityInfluence);
            
            //Fire
            Owner.ShootingComponent.Fire(CalculateLaunchVelocity(fireAtPos));
        
            return NodeStatus.Success;
        }

        private Vector3 CalculateLaunchVelocity(Vector3 target)
        {
            Vector3 ownerFirePosition = Owner.ShootingComponent.FireTransform.position;
            const float fireArcPeak = TankShooting.fireArcPeak;

            //Get the difference in positions between the target and this tank
            float displacementY = target.y - ownerFirePosition.y;
            Vector3 displacementXZ =
                new Vector3(target.x - ownerFirePosition.x, 0, target.z - ownerFirePosition.z);

            //Calculate Vertical and Horizontal Velocity
            //V(up) = sqrt(-2gh) from Kinematic Equasions
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * fireArcPeak);
            //V(right) = Px / srqt(-(2h)/g) + sqrt(2(Py - h) / g) from Kinematic Equasions
            Vector3 velocityXZ =
                displacementXZ / (Mathf.Sqrt(-2 * fireArcPeak / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - fireArcPeak)/Physics.gravity.y));

            return velocityY + velocityXZ;
        }
    
    }
}
