using System.Collections.Generic;

namespace AI.GOAP.Agent
{
    public class SubGoal
    {
        public Dictionary<string, bool> SubGoals { get; }
        public bool RemoveOnComplete { get; }

        /// <summary>
        /// Construct a sub goal
        /// </summary>
        /// <param name="key">Key of the goal</param>
        /// <param name="value">Value of the goal</param>
        /// <param name="removeOnComplete">Should this goal be removed after completion</param>
        public SubGoal(string key, bool value, bool removeOnComplete)
        {
            SubGoals = new Dictionary<string, bool>(1) {{key, value}};
            RemoveOnComplete = removeOnComplete;
        }
    }
}