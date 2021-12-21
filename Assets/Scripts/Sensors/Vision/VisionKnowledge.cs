using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using AI.Shared.Blackboard;
using Sensors;
using Sensors.Vision;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Component that combines all of the vision component of an agent in to a centeral repositiory
/// of vision knowledge 
/// </summary>
public class VisionKnowledge : MonoBehaviour
{
    #if UNITY_EDITOR
    public bool showVisionCones = false;
    #endif
    
    //Array of vision cone settings to use to create vision cones
    [SerializeField] private VisionConeSettings[] visionConesSettings;
    public IEnumerable<VisionConeSettings> VisionConeSettings => visionConesSettings;
    
    //Array of vision cones - created from settings
    private VisionCone[] visionCones;
        
    //Dictonary of objects seen by the vision cones and their awareness level
                        //Object, Awareness
    private Dictionary<BaseAgent, float> knownAgentAwarenessMap;
    
    //Last known position of last know agent with the time of last seen
    private Tuple<Vector3?, float> lastKnownAgentPos;
    
    //Owner agent of this component
    private BaseAgent owner;
    //Cahce blackboard for this agent
    protected TeamBlackboard teamBlackboard;
    
    //Static defines for the keys used to update the team blackboard
    public const string SeenAgentKey = "SeenAgentPosition";
    public const string LastKnownPosKey = "LastSeenAgentPos";
    
    protected virtual void Awake()
    {
        owner = GetComponent<BaseAgent>();
        teamBlackboard = TeamBlackboardManager.GetBlackboard(owner.Team);
    }

    // Start is called before the first frame update
    void Start()
    {
        knownAgentAwarenessMap = new Dictionary<BaseAgent, float>();
        lastKnownAgentPos = new Tuple<Vector3?, float>(null, -Mathf.NegativeInfinity);
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

    protected virtual void Update()
    {
        TickDownKnowledge();
        UpdateVisionCones();
        UpdateLastSeen();
        UpdateTeamBlackboard();
    }

    /// <summary>
    /// Update the team blackboard with relevant infomation
    /// </summary>
    private void UpdateTeamBlackboard()
    {
        (BaseAgent mostSeenAgent, float seenAgentConfidence) = GetMostSeenAgent();
        //If we have an agent that we can see update with this infomation, otherwise
        //if we have a last seen agent then update with that
        if (mostSeenAgent != null)
        {
            Vector3 seenAgentPosition = mostSeenAgent.transform.position;
            teamBlackboard.TryAddOrUpdateEntry(SeenAgentKey, seenAgentPosition, owner, seenAgentConfidence);
        }
    }

    /// <summary>
    /// Get Updated Data from all of the vision cones that this knowledge owns
    /// </summary>
    private void UpdateVisionCones()
    {
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
                //Don't add agent if they are on the same team
                if (agent.Team == owner.Team)
                {
                    continue;
                }
                
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
    /// Tick down all of the Knowledge values so that we loose sight over time 
    /// </summary>
    private void TickDownKnowledge()
    {
        const float tickDownPerSecond = 0.2f;
        //Remove from the list
        foreach (BaseAgent agent in knownAgentAwarenessMap.Keys.ToList())
        {
            knownAgentAwarenessMap[agent] -= tickDownPerSecond * Time.deltaTime;
            
            //If we have 0 or less awareness then we do not perceive this enemy anymore or agent is dead
            if (knownAgentAwarenessMap[agent] <= 0 || !agent.isActiveAndEnabled)
            {
                knownAgentAwarenessMap.Remove(agent);
            }
        }
    }

    /// <summary>
    /// Update the last seen agent position
    /// Determined by the 'most' seen agent - if we can see any at all
    /// </summary>
    private void UpdateLastSeen()
    {
        //If there are no agents visible then we cannot update
        if (knownAgentAwarenessMap.Count == 0)
        {
            return;
        }
        
        //Find the most seen agents
        KeyValuePair<BaseAgent, float> mostSeenAgentVis = new KeyValuePair<BaseAgent, float>(null, Mathf.NegativeInfinity); //Pair of the most seen agent and it's visibility
        foreach (KeyValuePair<BaseAgent,float> agentTimePair in knownAgentAwarenessMap)
        {
            if (agentTimePair.Value > mostSeenAgentVis.Value)
            {
                mostSeenAgentVis = agentTimePair;
            }
        }
        
        //Store the most seen agents position and now as the time we last saw it
        Vector3 lastSeenPos = mostSeenAgentVis.Key.transform.position;
        lastKnownAgentPos =
            new Tuple<Vector3?, float>(lastSeenPos, Time.timeSinceLevelLoad);
        
        //Update the team blackboard with this infomation
        teamBlackboard.TryAddOrUpdateEntry(LastKnownPosKey, lastSeenPos, owner);
    }

    /// <summary>
    /// Get the agent that we can see the most
    /// </summary>
    /// <returns></returns>
    public Tuple<BaseAgent, float> GetMostSeenAgent()
    {
        //Search all of the seen agents and get the most seen one
        Tuple<BaseAgent, float> mostSeen = new Tuple<BaseAgent, float>(null, Mathf.NegativeInfinity);
        foreach (KeyValuePair<BaseAgent,float> agentAwarenessPair in knownAgentAwarenessMap)
        {
            if (agentAwarenessPair.Value > mostSeen.Item2)
            {
                mostSeen = new Tuple<BaseAgent, float>(agentAwarenessPair.Key, agentAwarenessPair.Value);
            }
        }
        return mostSeen;
    }

    /// <summary>
    /// Get the last seen position of an agent - if no agents are visible
    /// </summary>
    public Tuple<Vector3?, float> GetLastSeenAgentPosition()
    {
        return lastKnownAgentPos;
    }

    /// <summary>
    /// Get all of the visible agents over a given threshold
    /// </summary>
    /// <param name="threshold">Vision threshold</param>
    public IEnumerable<BaseAgent> GetVisibleAgents(float threshold = 0)
    {
        //Loop all of the agents and check how visible they are
        List<BaseAgent> visibleAgentsOverThreshold = new List<BaseAgent>();
        foreach(KeyValuePair<BaseAgent, float> agentVisibilityPair in knownAgentAwarenessMap)
        {
            if(agentVisibilityPair.Value > threshold)
            {
                visibleAgentsOverThreshold.Add(agentVisibilityPair.Key);
            }
        }

        return visibleAgentsOverThreshold;
    }

    /// <summary>
    /// Invalidate the last known position
    /// </summary>
    public void InvalidateLKP()
    {
        //Clear LKP
        lastKnownAgentPos = new Tuple<Vector3?, float>(null, -Mathf.NegativeInfinity);
    }

    /// <summary>
    /// Reset our vision, removing all visible agents
    /// </summary>
    public void ResetVision()
    {
        knownAgentAwarenessMap?.Clear();
        InvalidateLKP();
    }
}

