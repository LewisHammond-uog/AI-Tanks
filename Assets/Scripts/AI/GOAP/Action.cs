using System.Collections.Generic;
using UnityEngine;

namespace AI.GOAP
{
    public abstract class Action : MonoBehaviour
    {
        [SerializeField] private string name = "Untitled action";
        [field: SerializeField] public float Cost { get; } = 1.0f;

        //Preconditions that must be fulfilled for out action to take place
        [SerializeField] private State[] preconditions;
        //Effects that happen once this action is completed
        [SerializeField] private State[] effects;
        public State[] Effects => effects;

        //Property to get the preconditions as a dictonary
        private Dictionary<string, object> preconditionsDictonary => State.AsDictionary(preconditions);
        private Dictionary<string, object> effectsDictonary => State.AsDictionary(Effects);

        //States local to the agent that is executing this action   
        private States agentStates;

        //Is this action running?
        private bool isRunning;
        
        //Running properties
        public enum ActionState
        {
            Fail,
            Success,
            Running
        }

        /// <summary>
        /// Perform this action
        /// </summary>
        /// <returns></returns>
        public ActionState Perform()
        {
            if (!isRunning)
            {
                if (!PrePerform())
                {
                    return ActionState.Fail;
                }
                isRunning = true;
            }

            ActionState state = Perform_Internal();

            //If we have completed the action then return success
            if (state == ActionState.Success)
            {
                PostPerform();
                isRunning = false;
            }

            return state;
        }
        
        
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
            //Check all this actions precondtions - if they are all in the conditions dictonary supplied then this
            //action is achiveable given those condtions
            foreach (KeyValuePair<string,object> pCondition in preconditionsDictonary)
            {
                if (!conditions.ContainsKey(pCondition.Key))
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract bool PrePerform();
        protected abstract ActionState Perform_Internal();
        protected abstract bool PostPerform();

    } 
}