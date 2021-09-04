using System;
using UnityEngine;
using UnityEngine.Audio;
using xOrfe.Utilities;

namespace Project.Scripts.Managers
{
    public sealed class SoundManager : SingletonUtility<SoundManager> , ISoundManagement
    {
        public void Reset()
        {
            throw new System.NotImplementedException();
        }
        
        public AudioMixerGroup mixerGroup;

        public Sound[] sounds;

        void Awake()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;

                s.source.outputAudioMixerGroup = mixerGroup;
            }
        }

        public void Play(string soundName)
        {
            Sound sound = GetSoundFromName(soundName);
            
            sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
            sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

            sound.source.Play();
        }
        
        public void Stop(string soundName)
        {
            Sound sound = GetSoundFromName(soundName);
            sound.source.Stop();
        }

        private Sound GetSoundFromName(string soundName)
        {
            Sound sound = Array.Find(sounds, item => item.name == soundName);
            if (sound == null) Debug.LogWarning("Sound: " + name + " not found!");


            return sound;
        }
    }
}