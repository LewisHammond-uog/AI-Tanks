using AI.BehaviourTree.BaseTypes;
using AI.BehaviourTree.BaseTypes.Nodes;


//Condition ot determine if we can fire
public class Condition_CanFire : ActionNode
{
    protected override NodeStatus Update_Internal()
    {
        //Ask the shooting component if we can fire
        return Owner.ShootingComponent.CheckIfFireAllowed() ? NodeStatus.Success : NodeStatus.Fail;
    }
}
