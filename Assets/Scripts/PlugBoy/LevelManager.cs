using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlugBoy
{

    public sealed class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Singleton;
        private float m_LoadingProgress;

        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        public float LoadingProgress
        {
            get
            {
                return m_LoadingProgress;
            }
        }

        public void LoadLevel()
        {
            // Scene loading co-routine
            StartCoroutine(LoadScenes());
        }

        private IEnumerator LoadScenes()
        {
            // yield return SceneManager.LoadSceneAsync("Loading");
            // yield return After
            // Start loading Game as soon as the loading screen finished loading
            yield return StartCoroutine(LoadLevel("Level1"));
        }

        private IEnumerator LoadLevel(string sceneName)
        {
            AsyncOperation asyncScene = SceneManager.LoadSceneAsync(sceneName);

            // stops the scene from displaying when it's finished loading
            asyncScene.allowSceneActivation = false;

            while (!asyncScene.isDone)
            {
                m_LoadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100f;

                // Scene has loaded as much as possible
                if (asyncScene.progress >= 0.9f)
                {
                    // Show scene
                    asyncScene.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private IEnumerator LoadAfterTimer()
        {
            // the reason we use a coroutine is simply to avoid a quick "flash" of the 
            // loading screen by introducing an artificial minimum load time
            yield return new WaitForSeconds(2.0f);
        }

    }

}