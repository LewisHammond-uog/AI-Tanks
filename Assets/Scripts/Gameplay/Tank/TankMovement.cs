using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public float speed = 12f;                 // How fast the tank moves forward and back.
        public float turnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public float acceleration = 0.5f; //How fast the tank accelerates
        
        #region Audio
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        #endregion //Audio
        
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.

        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        private NavMeshAgent movementAgent; //Navmesh Agent used for Navigation
        
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
            
            SetDestination(new Vector3(0,0,0),true);
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


        private void Start ()
        {
            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }


        private void Update ()
        {
            //EngineAudio ();
        }

        /// <summary>
        /// Set the destination for the movement, optionally start moving
        /// </summary>
        /// <param name="destination">Agent Destination</param>
        /// <param name="startMovement">If movement should be started niw</param>
        public void SetDestination(Vector3 destination, bool startMovement = false)
        {
            if (movementAgent)
            {
                movementAgent.SetDestination(destination);
                movementAgent.isStopped = false;
            }
        }


        //todo move to own class
        /*
        private void EngineAudio ()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }*/
    }
}