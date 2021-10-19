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

        /// <summary>
        /// Create a clone of this node and it's child
        /// </summary>
        /// <returns></returns>
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}