using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Characters;

namespace PlugBoy.Collectables
{
    public class Battery : Collectable
    {
        [SerializeField]
        protected int m_InstantChargePercent = 50;
        [SerializeField]
        protected bool m_RemoveAfter = true;
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
        // protected PlugBoyCharacter m_Player;

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

        // private void Start()
        // {
        //     m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlugBoyCharacter>();
        // }

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
            if (m_UseOnTriggerEnter2D && other.tag == "Player")
            {
                Collect();
            }
        }

        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (!m_UseOnTriggerEnter2D && coll.gameObject.tag == "Player")
            {
                Collect();
            }
        }

        public override void Collect()
        {
            GameManager.Singleton.ExternalCharge(m_InstantChargePercent);
            m_Animator.SetTrigger(COLLECT_TRIGGER);
            m_ParticleSystem.Play();
            if (m_RemoveAfter)
            {
                m_SpriteRenderer.enabled = false;
                Destroy(gameObject, m_ParticleSystem.main.duration);
            }
            m_Collider2D.enabled = false;
            AudioManager.Singleton.PlayBatterySound(transform.position);
        }
    }
}