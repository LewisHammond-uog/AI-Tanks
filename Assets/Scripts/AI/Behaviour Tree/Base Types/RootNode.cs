﻿using UnityEditor;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    public class RootNode : Node, IHasChild
    {
        [SerializeField] private Node child;

        protected override void OnEnterNode()
        {

        }

        protected override NodeStatus Update_Internal()
        {
            return child.Update();
        }

        protected override void OnExitNode()
        {

        }

        public Node GetChild()
        {
            return child;
        }

        public void SetChild(Node newChild)
        {
            child = newChild;
        }

        /// <summary>
        /// Get a clone of this node and children
        /// </summary>
        /// <returns></returns>
        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}