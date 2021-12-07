using System.Collections.Generic;

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
            public Dictionary<string, bool> State { get; }
            public Action Action { get; }

            public Node(Node parent, float cost, Dictionary<string, bool> allStates, Action action)
            {
                this.Parent = parent;
                this.Cost = cost;
                this.State = new Dictionary<string, bool>(allStates);
                this.Action = action;
            }
        }
    }
}