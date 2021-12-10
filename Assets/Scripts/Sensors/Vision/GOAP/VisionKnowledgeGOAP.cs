using System;
using System.Linq;
using AI.GOAP.Agent;
using UnityEngine;

namespace Sensors.Vision.GOAP
{
    public class VisionKnowledgeGOAP : VisionKnowledge
    {
        private GOAPAgent owner;
        
        private void Awake()
        {
            owner = GetComponent<GOAPAgent>();
        }

        protected override void Update()
        {
            //Vision Knowlage functionality
            base.Update();
            
            //If we have an enemy that we can see then update property
            owner.ModifyBelief("CanSeeEnemy", GetVisibleAgents().Any());
        }
    }
}