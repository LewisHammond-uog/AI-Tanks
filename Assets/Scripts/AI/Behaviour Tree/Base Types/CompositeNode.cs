using System.Collections.Generic;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class CompositeNode : Node
    {
        protected LinkedList<Node> children;
        
        protected CompositeNode(Agent owner) : base(owner)
        {
            children = new LinkedList<Node>();
        }
        
        /// <summary>
        /// Add a child to the composite node at the end of the list
        /// </summary>
        /// <param name="node">Node to add to children</param>
        public void AddChild(Node node)
        {
            children.AddLast(node);
        }
    }
}