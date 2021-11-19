using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Sensors;
using Sensors.Vision;
using UnityEngine;

/// <summary>
/// Component that combines all of the vision component of an agent in to a centeral repositiory
/// of vision knowledge 
/// </summary>
public class VisionKnowledge : MonoBehaviour
{
    //Array of vision cone settings to use to create vision cones
    [SerializeField] private VisionConeSettings[] visionConesSettings;
    public IEnumerable<VisionConeSettings> VisionConeSettings => visionConesSettings;
    
    //Array of vision cones - created from settings
    private VisionCone[] visionCones;
        
    //Dictonary of objects seen by the vision cones and their awareness level
                        //Object, Awareness
    private Dictionary<BaseAgent, float> knownAgentAwarenessMap;

    // Start is called before the first frame update
    void Start()
    {
        knownAgentAwarenessMap = new Dictionary<BaseAgent, float>();
        InitializeVisionCones();  
    }

    /// <summary>
    /// Init vision cone components from the Vision Cone Settings
    /// </summary>
    private void InitializeVisionCones()
    {
        visionCones = new VisionCone[visionConesSettings.Length];
        for (int i = 0; i < visionConesSettings.Length; i++)
        {
            VisionConeSettings conesSetting = visionConesSettings[i];
            VisionCone cone = gameObject.AddComponent<VisionCone>();
            cone.VisionConeSettings = conesSetting;
            visionCones[i] = cone;
        }
    }

    private void Update()
    {
        UpdateVisionCones();
    }

    /// <summary>
    /// Get Updated Data from all of the vision cones that this knowledge owns
    /// </summary>
    private void UpdateVisionCones()
    {
        knownAgentAwarenessMap.Clear();
        
        //Update each vision cone and then collect it's targets
        foreach (VisionCone visionCone in visionCones)
        {
            if (visionCone == null)
            {
                return;
            }
            
            visionCone.CalculateVisibleTargets();

            foreach (BaseAgent agent in visionCone.VisibleAgents)
            {
                //Check if agent already exists in map then choose the higher awareness level - deals with
                //if we are in multiple vision cones at once
                if (knownAgentAwarenessMap.ContainsKey(agent))
                {
                    float higherAwareness = Mathf.Max(knownAgentAwarenessMap[agent], visionCone.coneSettings.Weight);
                    knownAgentAwarenessMap[agent] = higherAwareness;
                }
                else
                {
                    //Otherwise add to the map with the awareness weight
                    knownAgentAwarenessMap.Add(agent, visionCone.coneSettings.Weight);
                }
            }
            
        }
    }

    /// <summary>
    /// Get all of the visible agents over a given threshold
    /// </summary>
    /// <param name="threshold">Vision threshold</param>
    public IEnumerable<BaseAgent> GetVisibleAgents(float threshold = 0)
    {
        //Loop all of the agents and check how visible they are
        List<BaseAgent> visibleAgentsOverThreshold = new List<BaseAgent>();
        foreach(var agentVisibilityPair in knownAgentAwarenessMap)
        {
            if(agentVisibilityPair.Value > threshold)
            {
                visibleAgentsOverThreshold.Add(agentVisibilityPair.Key);
            }
        }

        return visibleAgentsOverThreshold;
    }
}

