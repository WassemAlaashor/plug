using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugBoy.Utilities
{
    public class StickyPlatform : MonoBehaviour
    {
        private GameObject m_PlayerRoot;
        
        private Vector3 m_PreviousPosition;

        void Start()
        {
            // Player object is always in root
            m_PlayerRoot = GameObject.Find("/Player");
        }

        void Update()
        {
            m_PreviousPosition = transform.position;
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                m_PlayerRoot.transform.SetParent(transform);
                GameManager.Singleton.Player.OffsetPosition(transform.position - m_PreviousPosition);
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