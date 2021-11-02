using System;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Base class of agent used for shared functions between both types of AI
    /// </summary>
    public class BaseAgent : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("Test");
        }
    }
}