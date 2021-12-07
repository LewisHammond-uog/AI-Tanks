namespace AI.GOAP.Agent
{
    public class GOAPTankAgent : GOAPAgent
    {
        private void Start()
        {
            AddGoal("isAtEdge", true, 5);
        }
    }
}