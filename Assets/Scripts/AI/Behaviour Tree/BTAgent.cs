using System;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTrees
{
    public class BTAgent : AI.BaseAgent 
    {
        [SerializeField] private BehaviourTree treePrefab;
        public BehaviourTree RunningTree { get; private set; }

        private void Start()
        {
            //Clone the tree prefab so we have an unique instance that we can run
            if (treePrefab != null)
            {
                RunningTree = treePrefab.Clone();
                RunningTree.SetOwner(this);
            }
        }

        private void Update()
        {
            if (RunningTree)
            {
                RunningTree.Update();
            }
        }
    }
}