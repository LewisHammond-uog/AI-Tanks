using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankParticles : MonoBehaviour
{
    private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

    private void OnEnable()
    {
        // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
        // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
        // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Play();
        }
    }

    private void OnDisable()
    {
        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for(int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
    }
}
