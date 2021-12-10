using System;
using System.Collections.Generic;
using AI.GOAP.States;
using UnityEngine;
using UnityEngine.Analytics;
using Action = AI.GOAP.Actions.Action;

namespace AI.GOAP.Planner
{ 
    public partial class Planner
    {
        /// <summary>
        /// Create a plan
        /// </summary>
        /// <param name="actions">List of actions that can be perfomed in the plan</param>
        /// <param name="goal">Goal that this agent wants to plan to execute</param>
        /// <param name="agentStates">Beliefs of the agent that is using this plan</param>
        /// <param name="world">World that this plan is being executed in</param>
        /// <returns></returns>
        public Queue<Action> Plan(List<Action> actions, Dictionary<string, object> goal, StateCollection agentStates, World world)
        {
            //Check what actions are currently achiveable
            List<Action> usableActions = new List<Action>();
            foreach (Action action in actions)
            {
                if (action.IsAchievable())
                {
                    usableActions.Add(action);
                }
            }

            //Create the start of the tree with an entry node
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, world, agentStates, null);

            bool success = BuildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                Debug.Log("No Valid Plan");
                return null;
            }

            //Find the cheapest leaf
            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else
                {
                    if (leaf.Cost < cheapest.Cost)
                    {
                        cheapest = leaf;
                    }
                }
            }
            
            //We have found the cheapest leaf that is at the end of the plan we now need to work our way back up the tree
            List<Action> resultPlan = new List<Action>();
            Node currentNode = cheapest;
            while (currentNode != null)
            {
                if (currentNode.Action != null)
                {
                    resultPlan.Insert(0, currentNode.Action);
                }
                currentNode = currentNode.Parent;
            }
            
            //Create queue of actions
            Queue<Action> queue = new Queue<Action>(resultPlan);
            return queue;
        }

        /// <summary>
        /// Build a Graph of Actions 
        /// </summary>
        /// <param name="parent">Start node of the graph</param>
        /// <param name="leaves"></param>
        /// <param name="actions">Action that this graph can use</param>
        /// <param name="goal">The goal that this graph should statisfy - dictonary of states</param>
        /// <returns></returns>
        private bool BuildGraph(Node parent, List<Node> leaves, List<Action> actions, Dictionary<string, object> goal)
        {
            bool foundPath = false;
            foreach (Action action in actions)
            {
                if (!action.IsAchievableGiven(parent.State)) continue;
                
                //Add the effects of the current action to the current state
                Dictionary<string, object> currentState = new Dictionary<string, object>(parent.State);
                foreach (State effect in action.Effects)
                {
                    if (!currentState.ContainsKey(effect.key))
                    {
                        currentState.Add(effect.key, effect.value);
                    }
                }

                Node nextNode = new Node(parent, parent.Cost + action.Cost, currentState, action);
                if (IsGoalAchieved(goal, currentState))
                {
                    leaves.Add(nextNode);
                    foundPath = true;
                }
                else
                {
                    List<Action> subActionSet = CreateActionSubset(actions, action);
                    bool recursiveFind = BuildGraph(nextNode, leaves, subActionSet, goal);
                    if (recursiveFind)
                    {
                        foundPath = true;
                    }
                }
            }

            return foundPath;
        }

        /// <summary>
        /// Creates a subset of actions excluding a given action
        /// </summary>
        /// <param name="actions">List of actions to remove from</param>
        /// <param name="actionToRemove">Action to remove from the list</param>
        /// <returns>List of actions with given action removed</returns>
        private List<Action> CreateActionSubset(List<Action> actions, Action actionToRemove)
        {
            List<Action> actionSubset = new List<Action>(actions);
            if (actionSubset.Contains(actionToRemove))
            {
                actionSubset.Remove(actionToRemove);
            }
            return actionSubset;
        }

        /// <summary>
        /// Does a state statisfy a given goal
        /// </summary>
        /// <param name="goal">Goal to statisfy</param>
        /// <param name="currentState">State to check</param>
        /// <returns>If goal is statisified</returns>
        private bool IsGoalAchieved(Dictionary<string, object> goal, Dictionary<string, object> currentState)
        {
            //Check if the current state contains all of the effects that the goal requires
            foreach (KeyValuePair<string,object> state in goal)
            {
                if (!currentState.ContainsKey(state.Key))
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}