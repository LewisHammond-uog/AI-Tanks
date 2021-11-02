using System;
using Codice.CM.Client.Differences;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Base class of agent used for shared functions between both types of AI
    /// </summary>
    public class BaseAgent : MonoBehaviour
    {
        //Components
        public TankMovement movementCompoent { get; private set; }

        private void Awake()
        {
            movementCompoent = GetComponent<TankMovement>();
        }
    }
}