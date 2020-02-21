using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Characters;

namespace PlugBoy.Collectables
{

    public class FinishCollider : MonoBehaviour
    {
        [SerializeField]
        FinishDoor m_FinishDoor;
        private BoxCollider2D m_Collider;

        void Awake()
        {
            StartCoroutine(WaitAndEnable());
            m_Collider = GetComponent<BoxCollider2D>();
            m_Collider.enabled = false;
        }

        private IEnumerator WaitAndEnable()
        {
            yield return new WaitForSecondsRealtime(2f);
            m_Collider.enabled = true;

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            m_FinishDoor.Open();
        }

    }

}