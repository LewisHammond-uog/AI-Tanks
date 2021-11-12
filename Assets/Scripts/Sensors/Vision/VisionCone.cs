using System;
using System.Collections.Generic;
using AI;
using Sensors.Vision;
using UnityEngine;

namespace Sensors
{
    /// <summary>
    /// Represents a single vision cone in the world a stores a list of objects that are in the vision cone
    /// </summary>
    public class VisionCone : MonoBehaviour
    {
        [SerializeField] private VisionConeSettings coneSettings;
        public VisionConeSettings VisionConeSettings => coneSettings;

        //List of all visible targets from the last target pass
        private List<BaseAgent> visibleAgents;
        
        private void Start()
        {
            visibleAgents = new List<BaseAgent>();
        }

        /// <summary>
        /// Calculate all of the targets within the view cone
        /// </summary>
        public void CalculateVisibleTargets()
        {
            visibleAgents.Clear();
        
            //Get all of the agents within a sphere
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, coneSettings.ViewAngle, coneSettings.LayerMask);
            if (hitColliders.Length == 0)
            {
                return;
            }
            
            //Process all of the colliders that we hit and calculate the angle between us and them - to see if 
            //we are in the vision cone if we are then check that nothing is in the way
            foreach (Collider hitCollider in hitColliders)
            {
                //Abort if the collider is us
                GameObject hitGO = hitCollider.gameObject;
                if (hitGO == gameObject)
                {
                    continue;
                }

                //Check we are inside view cone - angle to target it within viewAngle/2
                Vector3 directionToTarget = (hitGO.transform.position - transform.position).normalized;
                float halfViewAngle = coneSettings.LayerMask * 0.5f;
                if (Vector3.Angle(transform.forward, directionToTarget) < halfViewAngle)
                {
                    //Check that there is nothing obscuring out view
                    if (!Physics.Linecast(transform.position, hitGO.transform.position, ~coneSettings.LayerMask))
                    {
                        if (hitGO.TryGetComponent<BaseAgent>(out BaseAgent agent))
                        {
                            visibleAgents.Add(agent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a direction vector from an angle, relative to the agents forward
        /// </summary>
        /// <param name="angle">Angle (in degrees) to get the direction from</param>
        /// <param name="yOffset">Offset of the angle in the Y direction</param>
        /// <returns></returns>
        public static Vector3 DirectionFromAngle(float angle, float yOffset)
        {
            angle += yOffset;
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        }
    }
}

