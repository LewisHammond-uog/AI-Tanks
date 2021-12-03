using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class to represent a cover point in the world
/// </summary>
public class CoverPoint : MonoBehaviour
{
    private static readonly Color CoverPointDrawColour = Color.red;
    private static readonly float CoverPointDrawRadius = 0.3f;

    //List of all cover points in the level
    private static readonly List<CoverPoint> AllCoverPoints = new List<CoverPoint>();
    
    private void Awake()
    {
        //Add to cover points
        AllCoverPoints.Add(this);
    }

    private void OnDestroy()
    {
        //Remove from cover points
        AllCoverPoints.Remove(this);
    }

    /// <summary>
    /// Get the number of cover points in the level
    /// </summary>
    /// <returns></returns>
    public static int GetCoverPointCount()
    {
        return AllCoverPoints.Count;
    }
    
    /// <summary>
    /// Get all of the cover points in the level - as readonly
    /// </summary>
    /// <returns>Read Only collection of cover points</returns>
    public static List<CoverPoint> GetAllLevelCoverPoints()
    {
        return AllCoverPoints;
    }

    //Draw debug gizmo. Ff any cover points are selected then draw all cover points
    private void OnDrawGizmos()
    {
        //Render cover points if any cover point is selected
        bool shouldRender = Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent<CoverPoint>(out CoverPoint _);
        if (!shouldRender)
        {
            return;
        }

        Gizmos.color = CoverPointDrawColour;
        Gizmos.DrawWireSphere(transform.position, CoverPointDrawRadius);
        Gizmos.color = Color.white;
    }
}
