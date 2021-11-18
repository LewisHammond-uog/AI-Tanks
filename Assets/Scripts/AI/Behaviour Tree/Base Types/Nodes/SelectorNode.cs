using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes { 
    public class SelectorNode : CompositeNode
    {
        private Node currentNode;
        private int currentNodeIndex;

        protected override void OnEnterNode()
        {
            currentNodeIndex = 0;
            currentNode = children[currentNodeIndex];
        }

        protected override void OnExitNode()
        {
        }

        protected override NodeStatus Update_Internal()
        {
            //Execute the current node and report back it's status
            NodeStatus nodeResult = children[currentNodeIndex].Update();
            switch (nodeResult)
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Success:
                    return NodeStatus.Success;
                case NodeStatus.Fail:
                    //Move to the next node in the sequence
                    ++currentNodeIndex;
                    currentNode = currentNodeIndex < children.Count ? children[currentNodeIndex] : null;
                    break;
                default:
                    return NodeStatus.Fail;
            }

            //If next node is null then we are at the end of the list - success! otherwise keep running
            return currentNode == null ? NodeStatus.Success : NodeStatus.Running;
        }

    }
}