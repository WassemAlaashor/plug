using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugBoy.Utilities
{
    public class StickyPlatform : MonoBehaviour
    {
        private GameObject m_PlayerRoot;

        void Start()
        {
            // Player object is always in root
            m_PlayerRoot = GameObject.Find("/Player");
        }
        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                m_PlayerRoot.transform.SetParent(transform);
            }
        }
        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                m_PlayerRoot.transform.SetParent(null);
            }
        }
    }
}