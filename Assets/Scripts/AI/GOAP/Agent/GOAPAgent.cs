using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.GOAP.Agent
{
    public abstract class GOAPAgent : BaseAgent
    {
        [SerializeField] private List<Action> actions = new List<Action>();
        private Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        //States that are local to this agent
        private States agentBeliefs;
        
        //Planner and the action queue that we will execute
        private Planner.Planner planner;
        private Queue<Action> actionQueue;
        
        //The current goal and action of this agent
        private Action currentAction;
        private SubGoal currentGoal;

        public override void Awake()
        {
            base.Awake();
            
            //Collect all of the actions that are components of the object and feed that to the action list
            Action[] actionsOnObject = GetComponents<Action>();
            actions = actionsOnObject.ToList();

            //Create the agent beleilfs
            agentBeliefs = new States();
            
            //Set the owner of each action
            SetActionOwners();
        }

        private void Update()
        {
            //Execute Action
            if (currentAction != null)
            {
                Action.ActionState actionState = currentAction.Perform();
                //Set our current action to null after we have completed a action or
                //reset the goal if we have failed
                if (actionState == Action.ActionState.Success)
                {
                    currentAction = null;
                }else if (actionState == Action.ActionState.Fail)
                {
                    ResetGoal(false);
                }
            }
            
            //Set the next action if required
            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
            }

            //If the action queue is empty (and not null) then we have completed the goal
            if (IsGoalComplete())
            {
                ResetGoal(true);
            }
        }

        private void LateUpdate()
        {
            if (planner == null || actionQueue == null)
            {
                CreatePlan();
            }
        }

        /// <summary>
        /// Add a belief to this agent
        /// </summary>
        public void AddBelief(string key, bool value)
        {
            agentBeliefs.AddState(key, value);
        }

        /// <summary>
        /// Modify a belief on this agent
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ModifyBelief(string key, bool value)
        {
            agentBeliefs.SetStateValue(key, value);
        }

        /// <summary>
        /// Remove a belief from this agent
        /// </summary>
        /// <param name="key"></param>
        public void RemoveBelief(string key)
        {
            agentBeliefs.RemoveState(key);
        }

        /// <summary>
        /// Add a Goal to the agent
        /// </summary>
        /// <param name="key">They key for this goal</param>
        /// <param name="value">Value for the goal to be achived</param>
        /// <param name="priority">The priority for this goal to be completed</param>
        /// <param name="removeOnComplete">If this goal should be removed when completed</param>
        public void AddGoal(string key, bool value, int priority, bool removeOnComplete = false)
        {
            goals.Add(new SubGoal(key, value, removeOnComplete), priority);
        }

        /// <summary>
        /// Set the owner of all of the acitons
        /// </summary>
        private void SetActionOwners()
        {
            foreach (Action action in actions)
            {
                action.SetOwner(this);
            }
        }
        
        /// <summary>
        /// Reset the current goal
        /// </summary>
        /// <param name="didComplete">If the goal was completed before this reset</param>
        public void ResetGoal(bool didComplete)
        {
            //Destroy the planner and remove goal if we completed
            if (didComplete && currentGoal.RemoveOnComplete)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        /// <summary>
        /// Create a plan for this agent to follow with the set goals
        /// </summary>
        private void CreatePlan()
        {
            planner = new Planner.Planner();
                
            //Sort our goals
            IOrderedEnumerable<KeyValuePair<SubGoal, int>> sortedGoals = from entry in goals
                orderby entry.Value descending
                select entry;

            //Try and plan our goals in priority order
            foreach (KeyValuePair<SubGoal,int> goalPriorityPair in sortedGoals)
            {
                actionQueue = planner.Plan(actions, goalPriorityPair.Key.SubGoals, agentBeliefs, World.GetInstance());
                if (actionQueue != null)
                {
                    currentGoal = goalPriorityPair.Key;
                    break;
                }
            }
        }
        
        /// <summary>
        /// Check if the current goal is completed
        /// </summary>
        private bool IsGoalComplete()
        {
            //If the action queue is not null but has 0 elements
            return actionQueue is {Count: 0} && currentAction != null;
        }
    }
}