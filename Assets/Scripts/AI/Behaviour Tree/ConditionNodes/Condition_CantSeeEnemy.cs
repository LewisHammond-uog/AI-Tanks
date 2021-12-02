

//A condition for if we can't see the enemy - inherits from Condition_CanSeeEnemy and just inverts the result

using AI.BehaviourTrees.BaseTypes;

public class Condition_CantSeeEnemy : Condition_CanSeeEnemy
{
    protected override NodeStatus Update_Internal()
    {
        //Run the base condition (Can See Enemy) if we succeded then we should fail and vice-verse
        NodeStatus canSeeEnemyStatus =  base.Update_Internal();
        return canSeeEnemyStatus == NodeStatus.Success ? NodeStatus.Fail : NodeStatus.Success;
    }
}
