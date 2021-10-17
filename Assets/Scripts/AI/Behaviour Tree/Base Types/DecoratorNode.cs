namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class DecoratorNode : Node
    {
        protected Node child;
        
        protected DecoratorNode(Agent owner) : base(owner)
        {
            child = null;
        }
    }
}