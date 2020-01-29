using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlugBoy
{

    public sealed class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Singleton;

        [SerializeField]
        private List<GameObject> m_LevelList;

        private int m_CurrentLevelIndex = 0;

        private GameObject m_CurrentLevel;
        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        void Start()
        {
            print(m_LevelList[0]);
        }

        private void LoadLevel(int levelIndex)
        {
            m_CurrentLevel = Instantiate(m_LevelList[levelIndex], Vector3.zero, Quaternion.identity);
            // GameManager.Singleton.Reset();
        }

        public void LoadCurrentLevel()
        {
            LoadLevel(m_CurrentLevelIndex);
        }


        public void LoadNextLevel()
        {
            if (m_CurrentLevelIndex < m_LevelList.Count - 1)
            {
                m_CurrentLevelIndex++;
                LoadCurrentLevel();
            }
        }

        public void ResetLevel()
        {
            Destroy(m_CurrentLevel);
            LoadCurrentLevel();
        }

    }

}