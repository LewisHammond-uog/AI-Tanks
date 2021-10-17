using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTrees;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class Node : ScriptableObject
    {
        protected Agent owner;
        private bool isRunning;
        
        //Store a struct of infomation that tells the BT editor about this nodes GUID and location in the graph
        public string guid;
        public Vector2 position;

        protected Node(Agent owner)
        {
            isRunning = false;
            this.owner = owner;
        }
        
        /// <summary>
        /// Update this node - running its logic
        /// </summary>
        /// <returns>The status of this node on at the end of its execution</returns>
        public NodeStatus Update()
        {
            if (!isRunning)
            {
                OnEnterNode();
                isRunning = true;
            }
            
            //Do the update logic of this node
            NodeStatus status = Update_Internal();
            
            //If not running then we exit the node
            if (status != NodeStatus.Running)
            {
                OnExitNode();
                isRunning = false;
            }

            return status;
        }

        protected abstract void OnEnterNode();
        protected abstract NodeStatus Update_Internal();
        protected abstract void OnExitNode();
    }
}