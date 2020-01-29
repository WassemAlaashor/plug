using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PlugBoy
{

    public sealed class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Singleton;

        [SerializeField]
        private Animator m_Transition;
        [SerializeField]
        private float m_TransitionTime = 1.0f;

        private int m_CurrentLevelIndex = 0;

        private GameObject m_CurrentLevel;
        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        // void Start()
        // {
            // print(m_LevelList[0]);
        // }

        IEnumerator LoadLevel(int levelIndex)
        {
            // m_CurrentLevel = Instantiate(m_LevelList[levelIndex], Vector3.zero, Quaternion.identity);
            // GameManager.Singleton.Reset();
            m_Transition.SetTrigger("Start");

            yield return new WaitForSeconds(m_TransitionTime);

            SceneManager.LoadScene(levelIndex);
            m_CurrentLevelIndex = levelIndex;
        }

        public void LoadCurrentLevel()
        {
            LoadLevel(m_CurrentLevelIndex);
        }


        public void LoadNextLevel()
        {
            // if (m_CurrentLevelIndex < m_LevelList.Count - 1)
            // {
                StartCoroutine(LoadLevel(m_CurrentLevelIndex + 1));
            // }
        }

        public void ResetLevel()
        {
            // Destroy(m_CurrentLevel);
            // LoadCurrentLevel();
        }

    }

}