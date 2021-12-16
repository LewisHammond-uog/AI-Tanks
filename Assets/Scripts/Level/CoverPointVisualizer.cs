using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
//Class to visualize cover points 
public class CoverPointVisualizer : MonoBehaviour
{
    [SerializeField] private float gizmosRadius = 0.5f;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        //Draw all cover points
        foreach (CoverPoint point in GetComponentsInChildren<CoverPoint>())
        {
            Vector3 coverPointPos = point.transform.position;   
            Gizmos.DrawLine(transform.position, coverPointPos);
            Gizmos.DrawWireSphere(coverPointPos, gizmosRadius);
        }

        Gizmos.color = Color.white;
    }
}
#endif