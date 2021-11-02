using System;
using AI.BehaviourTrees.BaseTypes;
using AI.BehaviourTrees.TestNodes;
using UnityEngine;

namespace AI.BehaviourTrees
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private BehaviourTree treePrefab;
        public BehaviourTree RunningTree { get; private set; }

        private void Start()
        {
            //Clone the tree perfab so we have an unique instance that we can run
            RunningTree = treePrefab.Clone();
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