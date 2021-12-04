using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP
{
    public abstract class Action : MonoBehaviour
    {
        [SerializeField] private string name = "Untitled action";
        [SerializeField] private float cost = 1.0f;
        
        //Preconditions that must be fulfilled for out action to take place
        [SerializeField] private State[] preconditions;
        //Effects that happen once this action is completed
        [SerializeField] private State[] effects;

        //Property to get the preconditions as a dictonary
        private Dictionary<string, object> preconditionsDictonary => State.AsDictionary(preconditions);
        private Dictionary<string, object> effectsDictonary => State.AsDictionary(effects);

        //States local to the agent that is executing this action   
        private States agentStates;

        /// <summary>
        /// Is this action achievable in the current world and agent states?
        /// </summary>
        /// <returns></returns>
        public bool IsAchievable()
        {
            return true;
        }

        /// <summary>
        /// Is this action achievable given the current world and agents states plus additonally supplied conditions
        /// </summary>
        /// <returns></returns>
        public bool IsAchievableGiven(Dictionary<string, object> conditions)
        {
            //Need to loop the condtiions in this dictonary and then check that it contains all of
            //the keys in the pre condtions
        }

        public abstract bool PrePerform();
        public abstract bool PostPerform();

    } 
}