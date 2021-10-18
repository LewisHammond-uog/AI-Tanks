using JetBrains.Annotations;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class DecoratorNode : Node, IHasChild
    {
        protected Node child;
        
        protected DecoratorNode(Agent owner) : base(owner)
        {
            child = null;
        }

        public Node GetChild()
        {
            return child;
        }

        public void SetChild(Node newChild)
        {
            this.child = newChild;
        }
    }
}