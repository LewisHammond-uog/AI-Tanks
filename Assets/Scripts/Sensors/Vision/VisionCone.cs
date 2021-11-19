using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Sensors.Vision
{
    /// <summary>
    /// Represents a single vision cone in the world a stores a list of objects that are in the vision cone
    /// </summary>
    public class VisionCone : MonoBehaviour
    {
        [SerializeField] public VisionConeSettings coneSettings;

        public VisionConeSettings VisionConeSettings
        {
            get => coneSettings;
            set => coneSettings = value;
        }

        //List of all visible targets from the last target pass
        private List<BaseAgent> visibleAgents;
        public IEnumerable<BaseAgent> VisibleAgents => visibleAgents;

        private void Awake()
        {
            visibleAgents = new List<BaseAgent>();
        }

        /// <summary>
        /// Calculate all of the targets within the view cone
        /// </summary>
        public void CalculateVisibleTargets()
        {
            visibleAgents.Clear();
        
            //todo layermask is wrong
            //Get all of the agents within a sphere
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, coneSettings.ViewRadius, coneSettings.LayerMask);
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
                float halfViewAngle = coneSettings.ViewAngle * 0.5f;
                float angleToObject = Vector3.Angle(transform.forward, directionToTarget);
                if (angleToObject < halfViewAngle)
                {
                    //Check that there is nothing obscuring out view
                    RaycastHit hit;
                    if (!Physics.Linecast(transform.position, hitGO.transform.position, out hit, ~coneSettings.LayerMask))
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

