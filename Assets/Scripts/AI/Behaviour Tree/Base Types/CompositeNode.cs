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
    }
}