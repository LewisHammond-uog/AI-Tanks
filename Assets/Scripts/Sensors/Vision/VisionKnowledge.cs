using System;
using System.Collections;
using System.Collections.Generic;
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
    public VisionConeSettings[] VisionConeSettings => visionConesSettings;
    
    //Array of vision cones - created from settings
    private VisionCone[] visionCones;
        
    // Start is called before the first frame update
    void Start()
    {
        InitializeVisionCones();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Init vision cone components from the Vision Cone Settings
    /// </summary>
    private void InitializeVisionCones()
    {
        visionCones = new VisionCone[visionConesSettings.Length];
        foreach (VisionConeSettings conesSetting in visionConesSettings)
        {
            VisionCone cone = gameObject.AddComponent<VisionCone>();
            cone.VisionConeSettings = conesSetting;
        }
    }
    
}

