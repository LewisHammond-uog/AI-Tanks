using JetBrains.Annotations;

namespace AI.BehaviourTrees.BaseTypes
{
    public abstract class DecoratorNode : Node
    {
        public Node Child { get; set; }


        protected DecoratorNode(Agent owner) : base(owner)
        {
            Child = null;
        }
    }
}