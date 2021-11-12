﻿using System;
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
        private bool showRadius = true;
        private bool showTraceToVisibleTargets = true;

        private VisionKnowledge visionComponent;
        private GameObject visionObject;

        private void Reset()
        {
            visionComponent = target as VisionKnowledge;
            visionObject = visionComponent.gameObject;
        }

        private void OnSceneGUI()
        {
            foreach (VisionConeSettings visionComponentVisionConeSetting in visionComponent.VisionConeSettings)
            {
                DrawVisionCone(visionObject, visionComponentVisionConeSetting);
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
            Vector3 targetPosition = fromObject.transform.position;
            if (showCone)
            {
                Handles.color = cone.drawColour;
                Vector3 negHalfViewAngle = VisionCone.DirectionFromAngle(-cone.ViewAngle / 2, fromObject.transform.eulerAngles.y);

                Handles.DrawSolidArc(targetPosition, Vector3.up, negHalfViewAngle, cone.ViewAngle,
                    cone.ViewRadius);
            }

            if (showRadius)
            {
                //Draw a disc to show the vision radius
                Handles.color = cone.drawColour;
                Handles.DrawWireDisc(targetPosition, Vector3.up, cone.ViewRadius);
            }

            /*
            Handles.color = Color.green;
            if (showTraceToVisibleTargets && targetConeComp.VisibleAgents != null)
            {
                //Draw lines to visible targets
                foreach (BaseAgent agent in targetConeComp.VisibleAgents)
                {
                    Handles.DrawLine(targetConeComp.transform.position, agent.transform.position);
                }
            }
            */

            Handles.color = Color.white;
        }
        }
}