using System;
using System.Collections.Generic;
using AI.GOAP.Agent;
using AI.GOAP.Goals;
using AI.GOAP.States;
using AI.Shared;
using AI.Shared.Blackboard;
using UnityEngine;

namespace AI.GOAP.Actions
{
    public abstract class Action : MonoBehaviour
    {
        //Owner of this action
        protected GOAPAgent Owner { get; private set; }

        //List of actions that this goal contributes to
        [SerializeField] private List<Goal> contributesToGoals;
        public List<Goal> ContributesToGoals => contributesToGoals;
        [SerializeField] private float cost = 1.0f;
        public float Cost => cost;

        //Preconditions that must be fulfilled for out action to take place
        [SerializeField] private State[] preconditions;
        //Effects that happen once this action is completed
        [SerializeField] private State[] effects;
        public State[] Effects => effects;

        //Property to get the preconditions as a dictonary
        private Dictionary<string, object> preconditionsDictonary;
        private Dictionary<string, object> effectsDictonary;

        //States local to the agent that is executing this action   
        private StateCollection agentStates;

        //Is this action running?
        private bool isRunning;
        
        //This actions blackboard
        public AgentBlackboard AgentBlackboard { get; set; }
        
        //Running properties
        public enum ActionState
        {
            Fail,
            Success,
            Running
        }

        private void Start()
        {
            //Add all precondtions and effects to dictonary
            preconditionsDictonary = new Dictionary<string, object>();
            foreach (State precondition in preconditions)
            {
                preconditionsDictonary.Add(precondition.key, precondition.value);
            }

            effectsDictonary = new Dictionary<string, object>();
            foreach (State effect in effects)
            {
                effectsDictonary.Add(effect.key, effect.value);
            }

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
            if (state == ActionState.Success || state == ActionState.Fail)
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
            foreach (KeyValuePair<string,object> pCondition in preconditionsDictonary)
            {
                if (!conditions.ContainsKey(pCondition.Key))
                {
                    return false;
                }
                
                //todo replace state comparision
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