using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using UnityEngine;

namespace AI.BehaviourTrees.TestNodes
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        
        protected override void OnEnterNode()
        {
           
        }

        protected override NodeStatus Update_Internal()
        {
            Debug.Log($"OnUpdate {message}");
            return NodeStatus.Success;
        }

        protected override void OnExitNode()
        {

        }
    }
}