using System.Collections.Generic;
using AI.BehaviourTrees.BaseTypes;
using UnityEngine;

namespace AI.BehaviourTrees.ActionNodes
{
    public class Action_DetermineBestEnemy : ActionNode
    {
        [Header("Evaluation Values")]
        [Tooltip("Raise the distance to this power when evaluating how much score to deduct")]
        [SerializeField] private float distancePower = 1.1f;

        //Score to deduct from when determining best enemy
        private const float baseScore = 1000;

        protected override void OnEnterNode()
        {
            base.OnEnterNode();
            Blackboard.bestEnemyToAttack = null;
        }

        protected override NodeStatus Update_Internal()
        {
            //Loop all of the visible enemies - if they are on the enemy team then give them a score
            //store the best scoring enemy to shoot at
            KeyValuePair<BaseAgent, float> agentBestScorePair =
                new KeyValuePair<BaseAgent, float>(null, float.NegativeInfinity);
            foreach (BaseAgent agent in Owner.VisionKnowledgeComponent.GetVisibleAgents())
            {
                //If we are the same team then don't give us a score
                if (agent.Team == Owner.Team) continue;
                
                float score = ScoreEnemy(agent);
                if (score > agentBestScorePair.Value)
                {
                    agentBestScorePair = new KeyValuePair<BaseAgent, float>(agent, score);
                }
            }
            
            //If we don't have a best agent (is null) then there were no agents on the other team, fail
            if (agentBestScorePair.Key == null)
            {
                return NodeStatus.Fail;
            }
            
            //Set the best enemy to attack in the blackboard
            Blackboard.bestEnemyToAttack = agentBestScorePair.Key;
            return NodeStatus.Success;
        }

        private float ScoreEnemy(BaseAgent enemyAgent)
        {
            float totalScore = baseScore;

            if (enemyAgent == null)
            {
                return float.NegativeInfinity;
            }
            
            //Evaluate Distance
            totalScore -= Mathf.Pow(Vector3.Distance(Owner.transform.position, enemyAgent.transform.position), distancePower);
            
            //Evaluate Health - get health as a percentage
            totalScore -= enemyAgent.HealthComponent.m_CurrentHealth / enemyAgent.HealthComponent.m_StartingHealth * 100;

            return totalScore;
        }
    }
}