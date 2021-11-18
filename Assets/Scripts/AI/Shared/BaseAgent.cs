using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public AiTeam myTeam { private set; get; }

        //List of all AI spawned
        private static List<BaseAgent> allAgents = new List<BaseAgent>();

        //Components
        public TankMovement MovementCompoent { get; private set; }
        public TankShooting ShootingComponent { get; private set; }
        public TurretMovement TurretComponent { get; private set; }
        public VisionKnowledge VisionKnowledgeComponent { get; private set; }


        private void Awake()
        {
            MovementCompoent = GetComponent<TankMovement>();
            ShootingComponent = GetComponent<TankShooting>();
            TurretComponent = GetComponentInChildren<TurretMovement>();
            VisionKnowledgeComponent = GetComponent<VisionKnowledge>();
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
                if(agent.myTeam == team)
                {
                    agents.Add(agent);
                }
            }

            return agents;
        }
    }
}