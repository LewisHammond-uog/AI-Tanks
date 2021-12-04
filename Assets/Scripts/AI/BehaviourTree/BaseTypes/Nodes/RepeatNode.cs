namespace AI.BehaviourTree.BaseTypes.Nodes
{
    public class RepeatNode : DecoratorNode
    {
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