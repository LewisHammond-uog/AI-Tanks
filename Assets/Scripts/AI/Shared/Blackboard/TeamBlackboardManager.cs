using System.Collections.Generic;
using UnityEditor.UIElements;

namespace AI.Shared.Blackboard
{
    /// <summary>
    /// Manager class for blackboards, handles the blackboards for each team
    /// </summary>
    public static class TeamBlackboardManager
    {
        private static readonly Dictionary<AiTeam, TeamBlackboard> TeamBlackboards = new Dictionary<AiTeam, TeamBlackboard>();

        /// <summary>
        /// Get the blackboard for a given team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static TeamBlackboard GetBlackboard(AiTeam team)
        {
            return TeamBlackboards.ContainsKey(team) ? TeamBlackboards[team] : CreateBlackboard(team);
        }

        /// <summary>
        /// Create a Blackboard
        /// </summary>
        /// <param name="team">Team for blackboard</param>
        /// <returns>Team blackboard</returns>
        private static TeamBlackboard CreateBlackboard(AiTeam team)
        {
            TeamBlackboard blackboard = new TeamBlackboard(team);
            TeamBlackboards.Add(team, blackboard);
            return blackboard;
        }
    }
}