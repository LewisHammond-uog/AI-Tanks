using System;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        //todo private this
        public Node rootNode;
        public NodeStatus treeState;

        private void Awake()
        {
            treeState = NodeStatus.Running;
        }

        public NodeStatus Update()
        {
            return rootNode.Update();
        }
    }
}