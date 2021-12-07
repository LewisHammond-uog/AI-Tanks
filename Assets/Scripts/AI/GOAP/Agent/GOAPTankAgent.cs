namespace AI.GOAP.Agent
{
    public class GOAPTankAgent : GOAPAgent
    {
        private void Start()
        {
            goals.Add(new SubGoal("isAtEdge", true, false), 5);
        }
    }
}