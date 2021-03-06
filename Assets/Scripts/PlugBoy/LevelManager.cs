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

        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        void Awake()
        {
            if (m_Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            m_Singleton = this;
        }

        IEnumerator _LoadLevel(int levelIndex)
        {
            // Start transition
            m_Transition.SetTrigger("Start");
            // Wait for the transition
            SceneManager.LoadScene(levelIndex);
            yield return new WaitForSeconds(m_TransitionTime);
        }

        // public void LoadCurrentLevel()
        // {
        //     LoadLevel(SceneManager.GetActiveScene().buildIndex);
        // }

        public void LoadLevel(int levelIndex)
        {
            // Only level selector uses this function, so it's bounds-safe
            StartCoroutine(_LoadLevel(levelIndex));
        }


        public void LoadNextLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings - 1)
            {
                StartCoroutine(_LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
            else
            {
                // Send to main menu
                LoadMainMenu();
            }
        }

        public void ResetLevel()
        {
            StartCoroutine(_LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        public void LoadMainMenu()
        {
            StartCoroutine(_LoadLevel(0));
        }

        public void QuitGame()
        {
            // Only main menu uses this function

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }

    }

}