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

        IEnumerator LoadLevel(int levelIndex)
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


        public void LoadNextLevel()
        {
            print("LevelManager: LoadNextLevel");
            print("Current: " + SceneManager.GetActiveScene().buildIndex);
            print("Next: " + SceneManager.GetActiveScene().buildIndex + 1);
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                 StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }

        public void ResetLevel()
        {
            print("LevelManager: ResetLevel");
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

    }

}