namespace AI.BehaviourTrees.BaseTypes
{
    public class RootNode : Node, IHasChild
    {
        private Node child;

        public RootNode(Agent owner) : base(owner)
        {
        }

        protected override void OnEnterNode()
        {

        }

        protected override NodeStatus Update_Internal()
        {
            return child.Update();
        }

        protected override void OnExitNode()
        {

        }

        public Node GetChild()
        {
            return child;
        }

        public void SetChild(Node newChild)
        {
            child = newChild;
        }
    }
}