namespace AI.GOAP
{
    public class World
    {
        private WorldStates worldStates;
        
        public World()
        {
            worldStates = new WorldStates();
        }

        public WorldStates GetWorldStates()
        {
            return worldStates;
        }
    }
}