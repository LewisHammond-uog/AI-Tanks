using System;
using AI.BehaviourTrees.BaseTypes;
using AI.BehaviourTrees.TestNodes;
using UnityEngine;

namespace AI.BehaviourTrees
{
    public class Agent : MonoBehaviour
    {
        private BehaviourTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviourTree>();
            var log = ScriptableObject.CreateInstance<DebugLogNode>();
            log.message = "test";

            tree.rootNode = log;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}