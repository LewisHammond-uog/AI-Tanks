using System;
using AI;
using Sensors.Vision;
using UnityEditor;
using UnityEngine;

namespace Sensors.Editor
{
    /// <summary>
    /// Custom Editor of Vision Cone so we we can do debug visalizations
    /// </summary>
    [CustomEditor(typeof(VisionKnowledge))]
    public class VisionConeVisualization : UnityEditor.Editor
    {
        //Store refrences to seralized properties in the Vision Cone component
        private SerializedObject targetObjectAsSerializedObject;

        private bool showCone = true;
        private bool showRadius = false;
        private bool showTraceToVisibleTargets = true;

        private VisionKnowledge visionComponent;
        private GameObject visionObject;
        private void OnSceneGUI()
        {
            visionComponent = target as VisionKnowledge;
            if (visionComponent.showVisionCones == false)
            {
                return;
            }
            
            
            visionObject = visionComponent.gameObject;
            if (!visionComponent || !visionObject)
            {
                return;
            }
            
            if (visionComponent.VisionConeSettings != null)
            {
                foreach (VisionConeSettings visionComponentVisionConeSetting in visionComponent.VisionConeSettings)
                {
                    DrawVisionCone(visionObject, visionComponentVisionConeSetting);
                }
            }
        }

        /// <summary>
        /// Draw a vision cone based on vision cone settings
        /// </summary>
        /// <param name="fromObject">Object to draw the cone from</param>
        /// <param name="cone">Cone settings to draw based on</param>
        private void DrawVisionCone(GameObject fromObject, VisionConeSettings cone)
        {
            //Drag an arc from the left side of the view angle over to the right side of the view angle
            Vector3 targetPosition = cone.ConeTransform.position;
            if (showCone)
            {
                Handles.color = cone.drawColour;
                Vector3 negHalfViewAngle = VisionCone.DirectionFromAngle(-cone.ViewAngle / 2, cone.ConeTransform.eulerAngles.y);

                Handles.DrawSolidArc(targetPosition, Vector3.up, negHalfViewAngle, cone.ViewAngle,
                    cone.ViewRadius);
            }

            if (showRadius)
            {
                //Draw a disc to show the vision radius
                Handles.color = cone.drawColour;
                Handles.DrawWireDisc(targetPosition, Vector3.up, cone.ViewRadius);
            }

            Handles.color = Color.white;
        }
    }
}
