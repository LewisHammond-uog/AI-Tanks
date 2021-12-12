using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public enum AiTeam
    {
        Red,
        Blue
    }

    /// <summary>
    /// Base class of agent used for shared functions between both types of AI
    /// </summary>
    public class BaseAgent : MonoBehaviour
    {
        //My Team
        [SerializeField] private AiTeam myTeam;
        public AiTeam Team => myTeam;

        //List of all AI spawned
        private static List<BaseAgent> allAgents = new List<BaseAgent>();

        //Components
        public TankMovement MovementCompoent { get; private set; }
        public TankShooting ShootingComponent { get; private set; }
        public TurretMovement TurretComponent { get; private set; }
        public VisionKnowledge VisionKnowledgeComponent { get; private set; }
        public TankHealth HealthComponent { get; private set; }


        public virtual void Awake()
        {
            MovementCompoent = GetComponent<TankMovement>();
            ShootingComponent = GetComponent<TankShooting>();
            TurretComponent = GetComponentInChildren<TurretMovement>();
            VisionKnowledgeComponent = GetComponentInChildren<VisionKnowledge>();
            HealthComponent = GetComponent<TankHealth>();
        }

        private void Start()
        {
            //Add to the AI List
            allAgents.Add(this);
        }

        private void OnDestroy()
        {
            //Remove this AI
            allAgents.Remove(this);
        }

        /// <summary>
        /// Get all of the AI Agents
        /// </summary>
        /// <returns>IEnumerable of AI Agents</returns>
        public static IEnumerable<BaseAgent> GetAllAgents()
        {
            return allAgents;
        }

        /// <summary>
        /// Get all of the AI agents on a given team
        /// </summary>
        /// <returns>IEnumerable of AI Agents</returns>
        public static IEnumerable<BaseAgent> GetAgentsOfTeam(AiTeam team)
        {
            List<BaseAgent> agents = new List<BaseAgent>();
            //Loop and pickout agents from a team
            foreach(BaseAgent agent in allAgents)
            {
                if(agent.Team == team)
                {
                    agents.Add(agent);
                }
            }

            return agents;
        }
        
        /// <summary>
        /// Determine the best enemy for this agent to attack based on vision knowlage
        /// </summary>
        public BaseAgent DetermineBestEnemyToAttack()
        {
            //Loop all of the visible enemies - if they are on the enemy team then give them a score
            //store the best scoring enemy to shoot at
            KeyValuePair<BaseAgent, float> agentBestScorePair =
                new KeyValuePair<BaseAgent, float>(null, float.NegativeInfinity);
            foreach (BaseAgent agent in VisionKnowledgeComponent.GetVisibleAgents())
            {
                //If we are the same team then don't give us a score
                if (agent.Team == Team) continue;
                
                float score = ScoreEnemy(agent);
                if (score > agentBestScorePair.Value)
                {
                    agentBestScorePair = new KeyValuePair<BaseAgent, float>(agent, score);
                }
            }

            return agentBestScorePair.Key;
        }
        
        /// <summary>
        /// Score an enemy agent based on a number of factors
        /// </summary>
        /// <returns></returns>
        private float ScoreEnemy(BaseAgent enemyAgent)
        {
            const float baseScore = 1000f;
            float totalScore = baseScore;

            if (enemyAgent == null)
            {
                return float.NegativeInfinity;
            }
            
            //Evaluate Distance
            const float distancePower = 1.1f;
            totalScore -= Mathf.Pow(Vector3.Distance(transform.position, enemyAgent.transform.position), distancePower);
            
            //Evaluate Health - get health as a percentage
            totalScore -= enemyAgent.HealthComponent.m_CurrentHealth / enemyAgent.HealthComponent.m_StartingHealth * 100;

            return totalScore;
        }

        /// <summary>
        /// Get a random point on the Navmesh
        /// </summary>
        /// <param name="randomPos">[OUT] Random Position on the NavMesh</param>
        /// <param name="wanderRadius">Radius to search</param>
        /// <returns>If position was on the navmesh</returns>
        public bool GetRandomPointOnNavMesh(out Vector3 randomPos, float wanderRadius)
        {
            //Determine Random Point
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
        
            //See if it is on the navmesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                randomPos = hit.position;
                return true;
            }

            randomPos = Vector3.zero;
            return false;
        }
    }
}