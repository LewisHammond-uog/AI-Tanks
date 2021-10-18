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
        }

        private void Update()
        {
            tree.Update();
        }
    }
}