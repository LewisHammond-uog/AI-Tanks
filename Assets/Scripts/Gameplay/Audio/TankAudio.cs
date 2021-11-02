using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Class to deal with audio for the tank
public class TankAudio : MonoBehaviour
{
    private TankMovement tankMovement;
    
    [SerializeField] private AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    [SerializeField] private AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    [SerializeField] private AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    [SerializeField] private float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
    
    //Current Tank Speed 
    private float tankSpeed;
    
    private void Awake()
    {
        //Get the movement component because we use it's speed for calculating audio pitch
        tankMovement = GetComponent<TankMovement>();
        if (tankMovement)
        {
            tankSpeed = tankMovement.Speed;
        }
    }

    private void Start()
    {
        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        EngineAudio();
    }

    private void EngineAudio ()
    {
        tankSpeed = 0f;
        if (tankMovement)
        {
            tankSpeed = tankMovement.Speed;
        }
        
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs (tankSpeed) < 0.1f)
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
    }
}
