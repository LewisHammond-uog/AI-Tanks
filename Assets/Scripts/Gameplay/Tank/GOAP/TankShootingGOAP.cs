using System;
using AI.GOAP.Agent;
using UnityEngine;

namespace Complete.GOAP
{
    //Modification of the TankShooting class for GOAP
    public class TankShootingGOAP : TankShooting
    {
        private GOAPAgent owner;

        private void Awake()
        {
            owner = GetComponent<GOAPAgent>();
        }
        
        private void Update()
        {
            //Update agent state for if they can fire
            owner.ModifyBelief("CanFire", CheckIfFireAllowed());
        }
    }
}