using System;
using System.Collections.Generic;
using AI.GOAP.Agent;
using AI.GOAP.States;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public abstract class Action : MonoBehaviour
    {
        //Owner of this action
        protected GOAPAgent Owner { get; private set; }
        
        [SerializeField] private string actionName = "Untitled action";
        [field: SerializeField] public float Cost { get; } = 1.0f;

        //Preconditions that must be fulfilled for out action to take place
        [SerializeField] private State[] preconditions;
        //Effects that happen once this action is completed
        [SerializeField] private State[] effects;
        public State[] Effects => effects;

        //Property to get the preconditions as a dictonary
        private Dictionary<string, object> PreconditionsDictonary => State.AsDictionary(preconditions);
        private Dictionary<string, object> EffectsDictonary => State.AsDictionary(Effects);

        //States local to the agent that is executing this action   
        private StateCollection agentStates;

        //Is this action running?
        private bool isRunning;
        
        //This actions blackboard
        public Blackboard Blackboard { get; set; }
        
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
        /// Set the owner of this action
        /// </summary>
        /// <param name="ownerAgent"></param>
        public void SetOwner(GOAPAgent ownerAgent)
        {
            Owner = ownerAgent;
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
            foreach (KeyValuePair<string,object> pCondition in PreconditionsDictonary)
            {
                if (!conditions.ContainsKey(pCondition.Key))
                {
                    return false;
                }

                //Compare the values of the object types
                bool valuesAreNotEqual = false;
                if (pCondition.Value is IComparable)
                {
                    int boolCompareResults =
                        ((IComparable) pCondition.Value).CompareTo((IComparable) conditions[pCondition.Key]);
                    //If there are no differences then they are equal
                    if (boolCompareResults != 0)
                    {
                        valuesAreNotEqual = true;
                    }
                }
                
                bool containsKey = conditions.ContainsKey(pCondition.Key);
                if (containsKey && valuesAreNotEqual)
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