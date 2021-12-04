using System.Collections.Generic;

namespace AI.GOAP
{
    public class WorldStates
    {
        private Dictionary<string, object> worldStates;

        public WorldStates()
        {
            worldStates = new Dictionary<string, object>();
        }

        /// <summary>
        /// Check if this object has a given state
        /// </summary>
        /// <param name="key">Key of the state</param>
        /// <returns>If this object has this state</returns>
        public bool HasState(string key)
        {
            return worldStates.ContainsKey(key);
        }

        /// <summary>
        /// Add a state to be managed by this class
        /// </summary>
        /// <param name="key">Key of the state</param>
        /// <param name="value">Value of the state</param>
        public void AddState(string key, object value)
        {
            worldStates?.Add(key, value);
        }

        /// <summary>
        /// Change an existing states value.
        /// Adds the key if it doesn't already exist in the world states
        /// </summary>
        /// <param name="key">Key of item to change</param>
        /// <param name="value">Value to update to</param>
        public void SetStateValue(string key, object value)
        {
            if (HasState(key))
            {
                worldStates[key] = value;
            }
            else
            {
                AddState(key, value);
            }
        }

        /// <summary>
        /// Remove State
        /// </summary>
        /// <param name="key"></param>
        public void RemoveState(string key)
        {
            if (worldStates.ContainsKey(key))
            {
                worldStates.Remove(key);
            }
        }
        
        /// <summary>
        /// Get the value of a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Value stored in key, null if key does't exist</returns>
        public object GetStateValue(string key)
        {
            return worldStates.ContainsKey(key) ? worldStates[key] : null;
        }

        /// <summary>
        /// Get all of the world states
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetStates()
        {
            return worldStates;
        }
    }
}