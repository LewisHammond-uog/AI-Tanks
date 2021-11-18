using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTrees;
using UnityEngine;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class Node : ScriptableObject
    {
        public BTAgent Owner { get; set; }
        public Blackboard Blackboard { get; set; }

        public bool IsRunning { get; private set; }
        
        //Event for node status update
        public delegate void NodeUpdateEvent(in NodeStatus currentStatus);
        
        public event NodeUpdateEvent OnNodeUpdate;

        
        //Store a struct of infomation that tells the BT editor about this nodes GUID and location in the graph
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        
        /// <summary>
        /// Update this node - running its logic
        /// </summary>
        /// <returns>The status of this node on at the end of its execution</returns>
        public NodeStatus Update()
        {
            if (!IsRunning)
            {
                OnEnterNode();
                IsRunning = true;
            }
            
            //Do the update logic of this node
            NodeStatus status = Update_Internal();
            OnNodeUpdate?.Invoke(in status);
            
            //If not running then we exit the node
            if (status != NodeStatus.Running)
            {
                OnExitNode();
                IsRunning = false;
            }

            return status;
        }

        /// <summary>
        /// Get a clone of this node
        /// </summary>
        /// <returns></returns>
        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected virtual void OnEnterNode()
        {

        }
        protected abstract NodeStatus Update_Internal();

        protected virtual void OnExitNode()
        {

        }
    }
}