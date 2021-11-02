using System;
using UnityEngine;
using UnityEngine.UI;


public class TankShooting : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody m_Shell;                   // Prefab of the shell.
    [SerializeField] private Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    [SerializeField] private AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    [SerializeField] private AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    [SerializeField] private AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    
    private TurretMovement turret; //Child that controls the movement of the turret

    [Header("Timing")] 
    [SerializeField] private float timeBetweenShots; //The min time allowed between shots
    private float lastFireTime; //Time when the last fire was

    private void Awake()
    {
        turret = GetComponentInChildren<TurretMovement>();
    }

    private void Start ()
    {
        //Allow fire straight away
        lastFireTime = Time.timeSinceLevelLoad - timeBetweenShots;
    }
    
    /// <summary>
    /// Is this tank allowed to fire?
    /// </summary>
    /// <returns></returns>
    public bool CheckIfFireAllowed()
    {
        return lastFireTime <= (Time.timeSinceLevelLoad - timeBetweenShots);
    }
    
    /// <summary>
    /// Fire a shell from the tank
    /// </summary>
    /// <param name="launchForce">Force to launch from the tank (clamped between min an max values)</param>
    /// <param name="overrideTimeCheck">If we should ignore the fire rate of the tank (can fire before allowed)</param>
    public void Fire (float launchForce, bool overrideTimeCheck = false)
    {
        //Check that we are allowed to fire
        if (!CheckIfFireAllowed() && !overrideTimeCheck)
        {
            return;
        }

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = launchForce * m_FireTransform.forward; 

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();
    }
}