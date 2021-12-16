using System;
using AI.GOAP.Agent;
using UnityEngine;

namespace Complete.GOAP
{    
    //Modification of the TankHealth class for GOAP
    public class TankHealthGOAP : TankHealth
    {
        private GOAPAgent goapOwner;

        //Value below or equal to which we are considered to have low health
        [SerializeField] private float lowHealthValue = 45f;
        
        protected override void Awake()
        {
            base.Awake();
            goapOwner = GetComponent<GOAPAgent>();
        }
        
        private void Update()
        {
            //Update agent state for if they have low health
            goapOwner.ModifyBelief("LowHealth", m_CurrentHealth <= lowHealthValue);
        }
    }
}