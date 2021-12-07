namespace AI.GOAP.Agent
{
    public class GOAPTankAgent : GOAPAgent
    {
        public void Awake()
        {
            base.Awake();
            goals.Add(new SubGoal("isAlive", null, false), 5);
        }
    }
}