using System.Collections.Generic;
using AI.GOAP.Actions;
using UnityEngine;

namespace AI.GOAP.Goals
{
    [CreateAssetMenu(menuName = "GOAP/Goals/Create Goal", fileName = "New Goal")]
    public class Goal : ScriptableObject
    {
        //The states that must be fufilled to get to this goal
        [SerializeField] private List<State> goalStates;

        //If this goal should be removed from the agent when it is complete
        [SerializeField] private bool removeOnComplete;
        
        public Dictionary<string, object> SubGoals => State.AsDictionary(goalStates);
        public bool RemoveOnComplete => removeOnComplete;

        /// <summary>
        /// Construct a sub goal
        /// </summary>
        /// <param name="goalStates">States the goal must achive to be complete</param>
        /// <param name="removeOnComplete">Should this goal be removed after completion</param>
        public Goal(List<State> goalStates, bool removeOnComplete)
        {
            this.goalStates = goalStates;
            this.removeOnComplete = removeOnComplete;
        }
    }
}