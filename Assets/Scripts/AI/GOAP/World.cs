namespace AI.GOAP
{
    public class World
    {
        private States worldStates;
        
        public World()
        {
            worldStates = new States();
        }

        public States GetWorldStates()
        {
            return worldStates;
        }
    }
}