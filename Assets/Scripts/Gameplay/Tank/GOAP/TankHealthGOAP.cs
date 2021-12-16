using System;
using AI.GOAP.Agent;
using UnityEngine;

namespace Complete.GOAP
{    
    //Modification of the TankHealth class for GOAP
    public class TankHealthGOAP : TankHealth
    {
        private GOAPAgent owner;

        //Value below or equal to which we are considered to have low health
        [SerializeField] private float lowHealthValue = 45f;
        
        protected override void Awake()
        {
            base.Awake();
            owner = GetComponent<GOAPAgent>();
        }
        
        private void Update()
        {
            //Update agent state for if they have low health
            owner.ModifyBelief("LowHealth", m_CurrentHealth <= lowHealthValue);
        }
    }
}