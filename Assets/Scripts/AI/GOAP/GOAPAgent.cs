using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AI.GOAP.Planner;

namespace AI.GOAP
{
    public class SubGoal
    {
        public Dictionary<string, object> SubGoals { get; }
        public bool RemoveOnComplete { get; }

        /// <summary>
        /// Construct a sub goal
        /// </summary>
        /// <param name="key">Key of the goal</param>
        /// <param name="value">Value of the goal</param>
        /// <param name="removeOnComplete">Should this goal be removed after completion</param>
        public SubGoal(string key, object value, bool removeOnComplete)
        {
            SubGoals = new Dictionary<string, object>(1) {{key, value}};
            this.RemoveOnComplete = removeOnComplete;
        }
    }

    public class GOAPAgent : MonoBehaviour
    {
        private List<Action> actions = new List<Action>();
        private Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

        private Planner.Planner planner;
        private Queue<Action> actionQueue;
        private Action currentAction;
        private SubGoal currentGoal;

        private void Awake()
        {
            //Collect all of the actions that are components of the object and feed that to the action list
            Action[] actionsOnObject = GetComponents<Action>();
            actions = actionsOnObject.ToList();
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            //todo check that our goal is still achiveable - interuptions
            
            if (planner == null || actionQueue == null)
            {
                CreatePlan();
            }

            //Check is the action queue is empty, if it is then remove the goal if required
            if (actionQueue is {Count: 0})
            {
                if (currentGoal.RemoveOnComplete)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }
            
            //If we have actions to run then run them
            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                
            }
            
        }

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
                actionQueue = planner.Plan(actions, goalPriorityPair.Key.SubGoals, null, World.GetInstance());
                if (actionQueue != null)
                {
                    currentGoal = goalPriorityPair.Key;
                    break;
                }
            }
        }
    }
}