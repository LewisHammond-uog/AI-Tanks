using System;
using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;
using Sensors.Hearing;
using UnityEngine;

namespace AI.BehaviourTrees.ConditionNodes
{
    /// <summary>
    /// Condition for if we have a sound drop nearby
    /// </summary>
    public class Condtion_CanHearEnemy : ActionNode
    {
        [Range(0,1)] [Tooltip("Amount (0-1) that we can hear a sound before considering it audable")]
        [SerializeField] private float hearingThreshold = 0.1f;
        
        protected override NodeStatus Update_Internal()
        {
            SoundDrop mostHeardSound = Owner.HearingKnowledgeComponent.GetMostHeardSound(hearingThreshold);
            if (mostHeardSound != null)
            {
                AgentBlackboard.investigatePosition = mostHeardSound.transform.position;
                return NodeStatus.Success;
            }

            return NodeStatus.Fail;
        }
    }
}