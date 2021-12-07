using System.Collections.Generic;

namespace AI.GOAP
{
    public class States
    {
        private Dictionary<string, bool> states;

        public States()
        {
            states = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Check if this object has a given state
        /// </summary>
        /// <param name="key">Key of the state</param>
        /// <returns>If this object has this state</returns>
        public bool HasState(string key)
        {
            return states.ContainsKey(key);
        }

        /// <summary>
        /// Add a state to be managed by this class
        /// </summary>
        /// <param name="key">Key of the state</param>
        /// <param name="value">Value of the state</param>
        public void AddState(string key, bool value)
        {
            states?.Add(key, value);
        }

        /// <summary>
        /// Change an existing states value.
        /// Adds the key if it doesn't already exist in the world states
        /// </summary>
        /// <param name="key">Key of item to change</param>
        /// <param name="value">Value to update to</param>
        public void SetStateValue(string key, bool value)
        {
            if (HasState(key))
            {
                states[key] = value;
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
            if (states.ContainsKey(key))
            {
                states.Remove(key);
            }
        }
        
        /// <summary>
        /// Get the value of a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Value stored in key, null if key does't exist</returns>
        public bool GetStateValue(string key)
        {
            return states[key];
        }

        /// <summary>
        /// Get all of the world states
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, bool> GetStates()
        {
            return states;
        }
    }
}