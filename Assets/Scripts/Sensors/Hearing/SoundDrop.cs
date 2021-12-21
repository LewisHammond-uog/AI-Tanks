using System;
using System.Collections;
using AI;
using UnityEditor;
using UnityEngine;

namespace Sensors.Hearing
{
    /// <summary>
    /// Object that represents a drop of sound
    /// </summary>
    public class SoundDrop : MonoBehaviour
    {
        public float lifeTime;
        public float volume;

        private void Start()
        {
            StartCoroutine(DestroyAfterTime(lifeTime));
        }

        /// <summary>
        /// Destroy this object after a given life time
        /// </summary>
        /// <param name="time">Time to wait</param>
        /// <returns></returns>
        private IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Destroy when an agent has touched us
            if (other.TryGetComponent(out BaseAgent agent))
            {
                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Draw a sphere to represent the sound
            const float gizmoRadius = 0.5f;
            Handles.color = Color.gray;
            Handles.DrawSolidDisc(transform.position, transform.up, gizmoRadius);
            Handles.color = Color.white;
        }
#endif
        
        
    }
}
