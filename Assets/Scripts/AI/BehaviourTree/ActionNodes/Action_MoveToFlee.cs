using AI.BehaviourTree.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTree.ActionNodes
{
    //Move the agent in it's flee direction
    public class Action_MoveToFlee : Action_MoveToPosition
    {
        [Tooltip("Distance to project the flee direction when attemping to project it far away")]
        [SerializeField] private float farProjectDistance = 10f;
        
        [Tooltip("Distance to project the flee direction when attemping to project it close")]
        [SerializeField] private float closeProjectDistance = 2f;

        //Have we already set a position on this node runthrough?
        private bool positionSet;

        protected override void OnEnterNode()
        {
            base.OnEnterNode();
            positionSet = false;
        }

        protected override NodeStatus Update_Internal()
        {
            if (positionSet == false)
            {
                //Project the flee direction, see if we can go far, then less far
                if (Owner.MovementCompoent.SetDestination(Blackboard.fleeDirection * farProjectDistance))
                {
                    moveToPos = Owner.transform.position + Blackboard.fleeDirection * farProjectDistance;
                    positionSet = true;
                }
                else if (Owner.MovementCompoent.SetDestination(Blackboard.fleeDirection * closeProjectDistance))
                {
                    moveToPos = Owner.transform.position + Blackboard.fleeDirection * closeProjectDistance;
                    positionSet = true;
                }
                else
                {
                    //We cannot project less than our shortest flee distance we therefore cannot run away!
                    return NodeStatus.Fail;
                }
            }


            return base.Update_Internal();
        }
    }
}