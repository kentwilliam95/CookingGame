using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance => instance;

        public AudioSource musics;
        public AudioSource sfx;
        private void Awake()
        {
            instance = this;    
        }

        public void PlaySfx(AudioClip clip, float volume = 1)
        {
            sfx.PlayOneShot(clip, volume);
        }
    }
}
