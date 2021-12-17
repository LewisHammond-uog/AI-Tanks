using System.Collections.Generic;
using AI.GOAP.Actions;
using AI.GOAP.States;

namespace AI.GOAP.Planner
{
    public partial class Planner
    {
        /// <summary>
        /// Node is a private class of Planner used to create nodes in the graph
        /// </summary>
        private class Node
        {
            public Node Parent { get; }
            public float Cost { get; }
            //public Dictionary<string, object> State { get; }
            public Action Action { get; }
            
            /// <summary>
            /// Create a node in the planner graph
            /// </summary>
            /// <param name="parent">Parent node</param>
            /// <param name="cost">Cost of this node</param>
            /// <param name="allStates">All states to add to this node</param>
            /// <param name="action">Action that this node represents</param>
            public Node(Node parent, float cost, IDictionary<string, object> allStates, Action action)
            {
                this.Parent = parent;
                this.Cost = cost;
                this.State = null;
                this.Action = action;
            }

            /// <summary>
            /// Create anode in the planner graph
            /// </summary>
            /// <param name="parent">Parent Node</param>
            /// <param name="cost">Cost of this node</param>
            /// <param name="world">World that this node exists within</param>
            /// <param name="agentBeliefs">Beliefs of the agent that is using this plan</param>
            /// <param name="action">Action that this node represents</param>>
            public Node(Node parent, float cost, World world, StateCollection agentBeliefs, Action action)
            {
                this.Parent = parent;
                this.Cost = cost;
                this.State = null;
                this.Action = action;
                
                //Add agentBeliefs to state
                if (agentBeliefs == null) return;
                foreach (KeyValuePair<string, object> belief in agentBeliefs.GetStates())
                {
                    //State.Add(belief.Key, belief.Value);
                }

            }
        }
    }
}