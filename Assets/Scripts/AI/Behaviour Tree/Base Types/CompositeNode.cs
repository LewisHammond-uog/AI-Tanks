using System;
using System.Collections.Generic;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> children = new List<Node>();

        protected CompositeNode(Agent owner) : base(owner)
        {

        }

        /// <summary>
        /// Add a child to the composite node at the end of the list
        /// </summary>
        /// <param name="node">Node to add to children</param>
        public void AddChild(Node node)
        {
            children ??= new List<Node>();

            children.Add(node);
        }

        /// <summary>
        /// Remove a child from the composite node
        /// </summary>
        /// <param name="node">Node to removes</param>
        public void RemoveChild(Node node)
        {
            children ??= new List<Node>();
            children.Remove(node);
        }

        /// <summary>
        /// Get the children of this node
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetChildren()
        {
            children ??= new List<Node>();
            return children;
        }
    }
}