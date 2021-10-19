using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTrees.TestNodes
{
    public class DebugLogNode : ActionNode
    {
        public string message;
        
        protected override void OnEnterNode()
        {
            Debug.Log($"OnStart {message}");
        }

        protected override NodeStatus Update_Internal()
        {
            Debug.Log($"OnUpdate {message}");
            return NodeStatus.Success;
        }

        protected override void OnExitNode()
        {
            Debug.Log($"OnExit {message}"); 
        }
    }
}