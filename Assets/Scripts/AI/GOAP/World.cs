namespace AI.GOAP
{
    public class World
    {
        private States worldStates;
        private static World instance;

        public static World GetInstance()
        {
            return instance ??= new World();
        }
        
        private World()
        {
            worldStates = new States();
        }

        public States GetWorldStates()
        {
            return worldStates;
        }
    }
}