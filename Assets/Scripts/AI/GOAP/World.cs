using AI.GOAP.States;

namespace AI.GOAP
{
    public class World
    {
        private StateCollection worldStates;
        private static World instance;

        public static World GetInstance()
        {
            return instance ??= new World();
        }
        
        private World()
        {
            worldStates = new StateCollection();
        }

        public StateCollection GetWorldStates()
        {
            return worldStates;
        }
    }
}