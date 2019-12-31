using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlugBoy
{

    public sealed class LevelManager : MonoBehaviour
    {
        private static LevelManager m_Singleton;

        public static LevelManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

    }

}