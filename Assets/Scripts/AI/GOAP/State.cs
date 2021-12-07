using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AI.GOAP
{
    [System.Serializable]
    public class State
    {
        public string key;
        public object value;


        /// <summary>
        /// Get a IEnumerable of States as a Dictonary
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>Dictonary of keys and values of the IEnumerable of states</returns>
        public static Dictionary<string, object> AsDictionary(IEnumerable<State> collection)
        {
            return collection.ToDictionary(state => state.key, state => state.value);
        }
    }
}