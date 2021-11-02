using UnityEngine;
using UnityEngine.AI;

public class TankMovement : MonoBehaviour
{
    [SerializeField] private float speed = 12f;                 // How fast the tank moves forward and back.
    [SerializeField] private float turnSpeed = 180f;            // How fast the tank turns in degrees per second.
    [SerializeField] private float acceleration = 0.5f; //How fast the tank accelerates

    private Rigidbody m_Rigidbody;              // Reference used to move the tank.

    private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

    private NavMeshAgent movementAgent; //Navmesh Agent used for Navigation

    public float Speed => movementAgent.velocity.magnitude;
    
    
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
        
        //Set the navmesh agents values as determined by the tank movement feilds
        movementAgent.speed = speed;
        movementAgent.angularSpeed = turnSpeed;
        movementAgent.acceleration = acceleration;
        
        
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;
        
        // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
        // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
        // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Play();
        }
    }


    private void OnDisable ()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;

        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for(int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
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
    
}
