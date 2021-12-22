namespace AI.BehaviourTree.BaseTypes.Nodes
{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnEnterNode()
        {
        }

        protected override NodeStatus Update_Internal()
        {
            child.Update();
            return NodeStatus.Running;
        }

        protected override void OnExitNode()
        {
        }
    }
}