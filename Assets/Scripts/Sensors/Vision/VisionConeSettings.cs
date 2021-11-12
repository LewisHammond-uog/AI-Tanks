using System;
using UnityEngine;

namespace Sensors.Vision
{
    /// <summary>
    /// Struct to store all of the settings of a vision cone
    /// </summary>
    [Serializable]
    public struct VisionConeSettings
    {
        //The Radius of the vision cone sight
        [Min(0)] [SerializeField] private float viewRadius;
        //The relative angle in which we can see in front of the agent
        [Range(0, 360)] [SerializeField] private float viewAngle;
        //The layer mask to use when looking for targets
        [SerializeField] private LayerMask targetLayerMask;

        //The Radius of the vision cone sight
        public float ViewRadius
        {
            set => viewRadius = Mathf.Min(0, value);
            get => viewRadius;
        }
        
        //The relative angle in which we can see in front of the agent
        public float ViewAngle
        {
            set => viewAngle = Mathf.Abs( value % 360); //Must be in the range 0 - 360
            get => viewAngle;
        }

        //Layer Mask of objects picked up by this cone
        public LayerMask LayerMask
        {
            get => targetLayerMask;
        }
    }
}