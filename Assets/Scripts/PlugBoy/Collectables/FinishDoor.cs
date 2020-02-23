using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Characters;

namespace PlugBoy.Collectables
{

    public class FinishDoor : Collectable
    {
        [SerializeField]
        protected ParticleSystem m_ParticleSystem;
        [SerializeField]
        protected SpriteRenderer m_SpriteRenderer;
        [SerializeField]
        protected Collider2D m_Collider2D;
        [SerializeField]
        protected Animator m_Animator;
        [SerializeField]
        protected bool m_UseOnTriggerEnter2D = true;

        protected bool m_Opened = false;
    
        public override SpriteRenderer SpriteRenderer
        {
            get
            {
                return m_SpriteRenderer;
            }
        }

        public override Animator Animator
        {
            get
            {
                return m_Animator;
            }
        }

        public override Collider2D Collider2D
        {
            get
            {
                return m_Collider2D;
            }
        }

        public override bool UseOnTriggerEnter2D
        {
            get
            {
                return m_UseOnTriggerEnter2D;
            }
            set
            {
                m_UseOnTriggerEnter2D = value;
            }
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            string tag = other.tag;
            print(tag);
            if (m_UseOnTriggerEnter2D && tag == "Player")
            {
                Collect();
            }
        }

        public override void OnCollisionEnter2D(Collision2D collision2D)
        {
            string tag = collision2D.gameObject.tag;
            if (!m_UseOnTriggerEnter2D && tag == "Player")
            {
                Collect();
            }
        }

        IEnumerator WaitForAnim()
        {
            yield return new WaitForSecondsRealtime(1.2f);
        }

        protected bool LevelCanEnd() => GameManager.Singleton.m_Coin.Value >= GameManager.Singleton.m_MaxCoin.Value;

        public void Open()
        {
            if (!m_Opened && LevelCanEnd())
            {
                m_Animator.SetTrigger(COLLECT_TRIGGER);
                m_Opened = true;
                StartCoroutine(WaitForAnim());
            }
        }

        public override void Collect()
        {
            // Check if all collectables were collected
            if (LevelCanEnd())
            {
                // AudioManager.Singleton.PlayCoinSound (transform.position);
                //m_ParticleSystem.Play();
                // m_SpriteRenderer.enabled = false;
                m_Collider2D.enabled = false;
                GameManager.Singleton.EndLevel();
                // Destroy(gameObject, m_ParticleSystem.main.duration);
            }
            else
            {
                // TODO: SHOW MESSAGE
                print("NOT READY YET!");
            }

        }
    }

}