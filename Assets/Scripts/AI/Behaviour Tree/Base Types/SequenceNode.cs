using System;
using System.Collections.Generic;
using AI.BehaviourTrees.BaseTypes;

namespace AI.BehaviourTrees.BaseTypes
{
    public class SequenceNode : CompositeNode
    {
        private LinkedListNode<Node> currentNode;
        
        public SequenceNode(Agent owner) : base(owner)
        {
        }

        protected override void OnEnterNode()
        {
            currentNode = children.First;
        }

        protected override NodeStatus Update_Internal()
        {
            //todo - just make a while loop?
            //Execute Child report back it's status
            NodeStatus nodeResult = currentNode.Value.Update();
            switch (nodeResult)
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Success:
                    //Move to the next child for the next update loop
                    currentNode = currentNode.Next;
                    break;
                case NodeStatus.Fail:
                    //Fall through
                default:
                    return NodeStatus.Fail;
            }
            
            //If next node is null then we are at the end of the linked list - success! otherwise keep running
            return currentNode == null ? NodeStatus.Success : NodeStatus.Running;

        }

        protected override void OnExitNode()
        {
        }
    }
}