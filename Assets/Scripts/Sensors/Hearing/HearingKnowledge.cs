using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors.Hearing
{
 
    /// <summary>
    /// Class that collects data about what the agent can hear
    /// </summary>
    public class HearingKnowledge : MonoBehaviour
    {
        //Radius at which we cannot hear anything
        [SerializeField] private float hearingRadius;
        [SerializeField] private LayerMask soundMask; //Layer(s) that the sound exists on
        
        //Dictonary of sounds and their perceived volumes
        private Dictionary<SoundDrop, float> soundVolumeMap;
        public Dictionary<SoundDrop, float> SoundsVolumeMap => soundVolumeMap;

        /// <summary>
        /// Polls the world for sounds, seeing if we can hear any of them
        /// </summary>
        private void CheckForSounds()
        {
            soundVolumeMap.Clear();
            
            //Do a sphere cast to find any sounds in the hearing radius
            Collider[] foundSounds = Physics.OverlapSphere(transform.position, hearingRadius, soundMask);
            if (foundSounds.Length == 0)
            {
                return;
            }
            
            //Calculate how much we can hear each sound
            foreach (Collider soundCollider in foundSounds)
            {
                //If there is not sound drop component then abort
                if (!soundCollider.TryGetComponent(out SoundDrop sound))
                {
                    return;
                }
                
                float localSoundVolume = CalculateHearingPercentage(sound.gameObject.transform.position);
                localSoundVolume *= sound.volume;
                
                //Check if the sound was heard through a wall - if so just reduce it by half
                if (Physics.Linecast(transform.position, sound.gameObject.transform.position, ~soundMask))
                {
                    localSoundVolume *= 0.5f;
                }
                
                //Add this sound to the list of sounds
                soundVolumeMap.Add(sound, localSoundVolume);
            }
        }
        
        /// <summary>
        /// Calculate the percentage of hearing base on the distance to the sound
        /// </summary>
        /// <returns>Distance to sound in the range 0 - 1, where 0 is at the edge of hearing and 1 is at the sound position</returns>
        private float CalculateHearingPercentage(Vector3 soundPosition)
        {
            float distanceToSound = Vector3.Distance(transform.position, soundPosition);
            return Mathf.InverseLerp(0, hearingRadius, distanceToSound);
        }
    }
}
