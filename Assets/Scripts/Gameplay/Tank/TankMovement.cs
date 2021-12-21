using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TankMovement : MonoBehaviour
{
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    
    private NavMeshAgent movementAgent; //Navmesh Agent used for Navigation

    public float Speed => movementAgent ? movementAgent.velocity.magnitude : 0f;

    public Vector3? Destination
    {
        get
        {
            if (movementAgent)
            {
                return movementAgent.destination;
            }
            else
            {
                return null;
            }
        }
    }
    
    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        movementAgent = GetComponent<NavMeshAgent>();
        if (!movementAgent)
        {
            Debug.LogError("No Navmesh Agent for TankMovement. Tank will not move and is likely to cause errors!");
        }
    }
    
    private void OnEnable ()
    {
        //Stop Navmesh Agent
        movementAgent.isStopped = true;
        
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        //Start coroutine to detect if we are stuck
        StartCoroutine(SolveStuck());
    }

    private void OnDisable ()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
        
        //Stop all coroutines
        StopAllCoroutines();
    }
    

    /// <summary>
    /// Set the destination for the movement, optionally start moving
    /// </summary>
    /// <param name="destination">Agent Destination</param>
    /// <param name="startMovement">If movement should be started niw</param>
    public bool SetDestination(Vector3 destination, bool startMovement = false)
    {
        bool didSet = false; //Did we set a valid destination?
        if (!movementAgent) return didSet;
        
        didSet = movementAgent.SetDestination(destination);
        if (startMovement)
        {
            movementAgent.isStopped = false;
        }

        return didSet;
    }

    /// <summary>
    /// Check if the movement agent has reached its destination
    /// </summary>
    /// <param name="threshold">Allowable distance to the destination to determine as reached</param>
    /// <returns>If we are at the destination</returns>
    public bool IsAtDestination(float threshold = 2f)
    {
        return Vector3.Distance(transform.position, movementAgent.destination) <= threshold;
    }

    private IEnumerator SolveStuck()
    {
        YieldInstruction wait = new WaitForSeconds(1f);
        while (true)
        {
            //Wait until we have zero veolcity and we should be going somewhere
            yield return wait;
            if (!movementAgent.pathPending && movementAgent.hasPath && movementAgent.velocity.sqrMagnitude == 0f)
            {
                //Stuck - reset
                Vector3 destination = movementAgent.destination;
                movementAgent.ResetPath();
                movementAgent.SetDestination(destination);
            }
        }
    }

}
