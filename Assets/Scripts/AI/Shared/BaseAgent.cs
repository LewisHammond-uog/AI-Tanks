using System;
using Codice.CM.Client.Differences;
using Complete;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Base class of agent used for shared functions between both types of AI
    /// </summary>
    public class BaseAgent : MonoBehaviour
    {
        //Components
        public TankMovement MovementCompoent { get; private set; }
        public TankShooting ShootingComponent { get; private set; }
        public TurretMovement TurretComponent { get; private set; }

        private void Awake()
        {
            MovementCompoent = GetComponent<TankMovement>();
            ShootingComponent = GetComponent<TankShooting>();
            TurretComponent = GetComponentInChildren<TurretMovement>();
        }
    }
}