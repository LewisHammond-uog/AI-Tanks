using System;
using System.Linq;
using AI.GOAP.Agent;
using UnityEngine;

namespace Sensors.Vision.GOAP
{
    public class VisionKnowledgeGOAP : VisionKnowledge
    {
        private GOAPAgent goapOwner;

        //Time that a LKP is valid for
        [SerializeField] private float allowedLKPTime = 5f;
        
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
            
        }

        private bool IsLKPValid(Tuple<Vector3?, float> lkpPair)
        {
            float timeSinceLastSeen = Time.timeSinceLevelLoad - lkpPair.Item2;
            return !(timeSinceLastSeen > allowedLKPTime) && lkpPair.Item1 != null;
        }
    }
}