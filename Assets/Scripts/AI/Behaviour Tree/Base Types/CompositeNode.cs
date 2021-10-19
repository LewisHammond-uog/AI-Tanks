using System;
using System.Collections.Generic;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class CompositeNode : Node, IHasChildren
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

        /// <summary>
        /// Sort children by a given comparision
        /// </summary>
        /// <param name="comparison">Comparision to use</param>
        public void SortChildren(Comparison<Node> comparison)
        {
            children?.Sort(comparison);
        }
        
        /// <summary>
        /// Create a clone of this node and it's children
        /// </summary>
        /// <returns></returns>
        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}