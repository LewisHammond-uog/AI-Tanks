using System;
using System.Linq;
using AI.GOAP.Agent;
using AI.Shared.Blackboard;
using UnityEngine;

namespace Sensors.Vision.GOAP
{
    public class VisionKnowledgeGOAP : VisionKnowledge
    {
        private GOAPAgent goapOwner;

        //Time that a LKP is valid for
        [SerializeField] private float allowedLKPTime = 5f;
        
        //Time that team knowledge is valid for
        [SerializeField] private float allowedTeamKnowledgeTime = 10f;
        
        protected override void Awake()
        {
            base.Awake();
            goapOwner = GetComponent<GOAPAgent>();
        }

        protected override void Update()
        {
            //Vision Knowlage functionality
            base.Update();
            
            //If we have an enemy that we can see then update property
            bool canSeeEnemy = GetVisibleAgents().Any(agent => agent.Team != goapOwner.Team);
            goapOwner.ModifyBelief("CanSeeEnemy", canSeeEnemy);
            
            //If we cannot see an enemy then we should update if we have a LKP
            Tuple<Vector3?, float> agentLKPTime = GetLastSeenAgentPosition();
            bool isLKPValid = IsLKPValid(agentLKPTime);
            goapOwner.ModifyBelief("RecentlySeenEnemy", isLKPValid);
            
            //Update if our team has seen an enemy recently, only update if we are not already in the area
            const float localDistance = 5f; //distance to be considered already at the position
            bool teamHasSeenEnemy = TeamSeenEnemy(out float distanceToEnemy);
            goapOwner.ModifyBelief("TeamSeenEnemy", teamHasSeenEnemy && distanceToEnemy > localDistance);
        }

        /// <summary>
        /// Evalulate if our team has recently seen an enemy
        /// </summary>
        /// <returns></returns>
        private bool TeamSeenEnemy(out float distance)
        {
            TeamBlackboard.Entry blackboardEntry;
            bool seenEnemy = false;
            distance = float.PositiveInfinity;
            
            seenEnemy = teamBlackboard.TryGetEntry(SeenAgentKey, allowedTeamKnowledgeTime, out blackboardEntry);
            if (blackboardEntry == null)
            {
                seenEnemy = teamBlackboard.TryGetEntry(LastKnownPosKey, allowedTeamKnowledgeTime, out blackboardEntry);
            }

            //If we had an entry record our distance to it
            if (blackboardEntry != null)
            {
                distance = Vector3.Distance(transform.position, blackboardEntry.Value);
            }
            
            return seenEnemy;
        }

        private bool IsLKPValid(Tuple<Vector3?, float> lkpPair)
        {
            float timeSinceLastSeen = Time.timeSinceLevelLoad - lkpPair.Item2;
            return !(timeSinceLastSeen > allowedLKPTime) && lkpPair.Item1 != null;
        }
    }
}