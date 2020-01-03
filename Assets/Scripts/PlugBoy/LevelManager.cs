using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlugBoy
{

    public sealed class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Singleton;

        [SerializeField]
        private List<GameObject> m_LevelList;
        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        public void LoadLevel(int levelNumber)
        {
            Instantiate(m_LevelList[levelNumber - 1], Vector3.zero, Quaternion.identity);
            GameManager.Singleton.Reset();
        }

    }

}