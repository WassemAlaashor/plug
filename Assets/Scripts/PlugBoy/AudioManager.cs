using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugBoy
{

    public class AudioManager : MonoBehaviour
    {

        #region Singleton

        private static AudioManager m_Singleton;

        public static AudioManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        #endregion

        #region Fields

        [Header("Audio Sources")]
        [Space]
        [SerializeField]
        protected AudioSource m_MusicAudioSource;
        [SerializeField]
        protected AudioSource m_SoundAudioSource;
        [SerializeField]
        protected AudioSource m_CoinAudioSource;
        [SerializeField]
        protected AudioSource m_ChargingAudioSource;
        [SerializeField]
        protected AudioSource m_DischargingAudioSource;
        [SerializeField]
        protected AudioSource m_DieAudioSource;
        [SerializeField]
        protected AudioSource m_UIAudioSource;

        [Header("Music Clips")]
        [Space]
        [SerializeField]
        protected AudioClip m_MusicClip;

        [Header("Sound Clips")]
        [Space]
        [SerializeField]
        protected AudioClip m_CoinSound;
        [SerializeField]
        protected AudioClip m_BatterySound;
        [SerializeField]
        protected AudioClip m_WaterSplashSound;

        [SerializeField]
        protected AudioClip[] m_GroundedSounds;
        [SerializeField]
        protected AudioClip m_JumpSound;
        [SerializeField]
        protected AudioClip[] m_FootstepSounds;
    
        [SerializeField]
        protected AudioClip m_ButtonClickSound;

        #endregion

        #region MonoBehaviour Messages

        void Awake()
        {
            m_Singleton = this;
            PlayMusic();
        }

        #endregion

        #region Methods

        public void PlayMusic()
        {
            m_MusicAudioSource.clip = m_MusicClip;
            m_MusicAudioSource.Play();
        }

        // TODO: Positional sound
        // public void PlaySoundAt(AudioClip clip, Vector3 position, float volume)
        // {
        //     AudioSource.PlayClipAtPoint(clip, position, volume);
        // }

        public void PlaySoundOn(AudioSource audio, AudioClip clip)
        {
            audio.clip = clip;
            audio.Play();
        }

        public void StartChargingSound(Vector3 position)
        {
            m_ChargingAudioSource.Play();
        }

        public void StopDischargingSound()
        {
            m_DischargingAudioSource.Stop();
        }

        public void StartDischargingSound(Vector3 position)
        {
            m_DischargingAudioSource.Play();
        }

        public void StopChargingSound()
        {
            m_ChargingAudioSource.Stop();
        }

        public void PlayCoinSound(Vector3 position)
        {
            PlaySoundOn(m_CoinAudioSource, m_CoinSound);
        }

        public void PlayBatterySound(Vector3 position)
        {
            PlaySoundOn(m_CoinAudioSource, m_BatterySound);
        }

        public void PlayWaterSplashSound(Vector3 position)
        {
            PlaySoundOn(m_DieAudioSource, m_WaterSplashSound);
        }

        public void PlayGroundedSound(AudioSource audio)
        {
            if (m_GroundedSounds.Length > 0)
            {
                PlaySoundOn(audio, GetRandomClip(m_GroundedSounds));
            }
        }

        public void PlayJumpSound(AudioSource audio)
        {
            PlaySoundOn(audio, m_JumpSound);
        }

        public void PlayFootstepSound(AudioSource audio)
        {
            if (m_FootstepSounds.Length > 0)
            {
                PlaySoundOn(audio, GetRandomClip(m_FootstepSounds));
            }
        }

        public void PlayFootstepSound(AudioSource audio, ref int index)
        {
            if (m_FootstepSounds.Length > 0)
            {
                PlaySoundOn(audio, m_FootstepSounds[index]);
                if (index < m_FootstepSounds.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
        }

        public void PlayClickSound()
        {
            PlaySoundOn(m_UIAudioSource, m_ButtonClickSound);
        }

        public AudioClip GetRandomClip(AudioClip[] clips)
        {
            return (clips.Length > 0) ? clips[Random.Range(0, clips.Length)] : null;
        }

        #endregion

    }

}