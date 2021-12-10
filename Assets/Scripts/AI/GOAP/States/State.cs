using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace AI.GOAP
{
    public abstract class State : ScriptableObject
    {
        public string key;
        public object value { protected set; get; }

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

    public abstract class StateWithType<T> : State
    {
        [SerializeField] private T typedValue;

        public T TypedValue
        {
            set
            {
                typedValue = value;
                this.value = value;
            }
        }
    }
}