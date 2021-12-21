using AI.BehaviourTree.BaseTypes;

namespace AI.BehaviourTrees.ConditionNodes
{
    /// <summary>
    /// Condition to check if our team has NOT seen an enemy
    /// </summary>
    class Condition_TeamNotSeenEnemy : Condition_TeamSeenEnemy
    {
        protected override NodeStatus Update_Internal()
        {
            //Run the base condition (Team Has Seen Enemy) if we succeded then we should fail and vice-verse
            NodeStatus canSeeStatus = base.Update_Internal();
            return canSeeStatus == NodeStatus.Success ? NodeStatus.Fail : NodeStatus.Success;
        }
    }
}