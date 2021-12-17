using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Complete;
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
    
    //Health heal rate
    [SerializeField] private float healPerSecond = 0.2f;
    //Maximum speed to allow healing
    [SerializeField] private float maxSpeed = 2f;

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

    private void OnTriggerStay(Collider other)
    {
        //OnTrigger, if we are stopped then increase our health
        if (other.gameObject.TryGetComponent(out TankHealth healthComp) && 
            other.gameObject.TryGetComponent(out TankMovement movementComp))
        {
            if (movementComp.Speed < maxSpeed)
            {
                healthComp.IncreaseHealth(healPerSecond * Time.deltaTime);
            }
        }
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
