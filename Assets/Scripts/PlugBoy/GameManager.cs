﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using PlugBoy.Characters;
using PlugBoy.Collectables;
using PlugBoy.TerrainGeneration;

namespace PlugBoy
{
    public sealed class GameManager : MonoBehaviour
    {
        public delegate void AudioEnabledHandler(bool active);

        public delegate void ScoreHandler(float newScore, float highScore, float lastScore);

        public delegate void ResetHandler();

        public static event ResetHandler OnReset;
        public static event ScoreHandler OnScoreChanged;
        public static event AudioEnabledHandler OnAudioEnabled;

        private static GameManager m_Singleton;

        public static GameManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        [SerializeField]
        private Character m_MainCharacter;
        private float m_StartScoreX = 0f;
        private float m_HighScore = 0f;
        private float m_LastScore = 0f;
        private float m_Score = 0f;

        private bool m_GameStarted = false;
        private bool m_GameRunning = false;
        private bool m_AudioEnabled = true;

        /// <summary>
        /// This is my developed callbacks compoents, because callbacks are so dangerous to use we need something that automate the sub/unsub to functions
        /// with this in-house developed callbacks feature, we garantee that the callback will be removed when we don't need it.
        /// </summary>
        public Property<int> m_Coin = new Property<int>(0);


        #region Getters
        public bool gameStarted
        {
            get
            {
                return m_GameStarted;
            }
        }

        public bool gameRunning
        {
            get
            {
                return m_GameRunning;
            }
        }

        public bool audioEnabled
        {
            get
            {
                return m_AudioEnabled;
            }
        }
        #endregion

        void Awake()
        {
            if (m_Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            m_Singleton = this;
            m_Score = 0f;
            m_Coin.Value = 0;
            SetAudioEnabled(true);
            m_LastScore = 0f;
            m_HighScore = 0f;
        }

        void UpdateDeathEvent(bool isDead)
        {
            if (isDead)
            {
                StartCoroutine(DeathCrt());
            }
            else
            {
                StopCoroutine("DeathCrt");
            }
        }

        IEnumerator DeathCrt()
        {
            m_LastScore = m_Score;
            if (m_Score > m_HighScore)
            {
                m_HighScore = m_Score;
            }
            if (OnScoreChanged != null)
            {
                OnScoreChanged(m_Score, m_HighScore, m_LastScore);
            }

            yield return new WaitForSecondsRealtime(1.5f);

            EndGame();
            // var endScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.END_SCREEN);
            // UIManager.Singleton.OpenScreen(endScreen);
        }

        private void Start()
        {
            m_MainCharacter.IsDead.AddEventAndFire(UpdateDeathEvent, this);
            m_StartScoreX = m_MainCharacter.transform.position.x;
            Init();
            StartGame();
        }

        public void Init()
        {
            EndGame();
            UIManager.Singleton.Init();
            StartCoroutine(Load());
        }

        void Update()
        {
            if (m_GameRunning)
            {
                if (m_MainCharacter.transform.position.x > m_StartScoreX && m_MainCharacter.transform.position.x > m_Score)
                {
                    m_Score = m_MainCharacter.transform.position.x;
                    if (OnScoreChanged != null)
                    {
                        OnScoreChanged(m_Score, m_HighScore, m_LastScore);
                    }
                }
            }
        }

        IEnumerator Load()
        {
            // var startScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.START_SCREEN);
            // yield return new WaitForSecondsRealtime(2f);
            var inGameScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.IN_GAME_SCREEN);
            yield return new WaitForSecondsRealtime(2f);
            UIManager.Singleton.OpenScreen(inGameScreen);
        }

        void OnApplicationQuit()
        {
            if (m_Score > m_HighScore)
            {
                m_HighScore = m_Score;
            }
            // SaveGame.Save<int>("coin", m_Coin.Value);
            // SaveGame.Save<float>("lastScore", m_Score);
            // SaveGame.Save<float>("highScore", m_HighScore);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ToggleAudioEnabled()
        {
            SetAudioEnabled(!m_AudioEnabled);
        }

        public void SetAudioEnabled(bool active)
        {
            m_AudioEnabled = active;
            AudioListener.volume = active ? 1f : 0f;
            if (OnAudioEnabled != null)
            {
                OnAudioEnabled(active);
            }
        }

        public void StartGame()
        {
            m_GameStarted = true;
            ResumeGame();
        }

        public void StopGame()
        {
            m_GameRunning = false;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            m_GameRunning = true;
            Time.timeScale = 1f;
        }

        public void EndGame()
        {
            m_GameStarted = false;
            StopGame();
            OnReset();
            StartGame();
        }

        public void RespawnMainCharacter()
        {
            RespawnCharacter(m_MainCharacter);
        }

        public void RespawnCharacter(Character character)
        {
            character.Reset();
        }

        public void Reset()
        {
            m_Score = 0f;
            if (OnReset != null)
            {
                OnReset();
            }
        }

        [System.Serializable]
        public class LoadEvent : UnityEvent
        {

        }

    }

}