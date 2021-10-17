using AI.BehaviourTrees.BaseTypes;

namespace AI.BehaviourTrees.TestNodes
{
    public class RepeatNode : DecoratorNode
    {
        public RepeatNode(Agent owner) : base(owner)
        {
        }

        protected override void OnEnterNode()
        {
        }

        protected override NodeStatus Update_Internal()
        {
            //todo customize behaviour
            child.Update();
            return NodeStatus.Running;
        }

        protected override void OnExitNode()
        {
        }
    }
}