﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Characters;

namespace PlugBoy.Collectables
{
	public class Coin : Collectable
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

		public override SpriteRenderer SpriteRenderer {
			get {
				return m_SpriteRenderer;
			}
		}

		public override Animator Animator {
			get {
				return m_Animator;
			}
		}

		public override Collider2D Collider2D {
			get {
				return m_Collider2D;
			}
		}

		public override bool UseOnTriggerEnter2D {
			get {
				return m_UseOnTriggerEnter2D;
			}
			set {
				m_UseOnTriggerEnter2D = value;
			}
		}

		public override void OnTriggerEnter2D (Collider2D other)
		{
			string tag = other.tag;
			print(tag);
			if (m_UseOnTriggerEnter2D && tag == "Player") {
				Collect ();
			}
		}

		public override void OnCollisionEnter2D (Collision2D collision2D)
		{
			string tag = collision2D.gameObject.tag;
			if (!m_UseOnTriggerEnter2D && tag == "Player") {
				Collect ();
			}
		}

		public override void Collect ()
		{
            GameManager.Singleton.m_Coin.Value++;
			m_Animator.SetTrigger (COLLECT_TRIGGER);
			m_ParticleSystem.Play ();
			m_SpriteRenderer.enabled = false;
			m_Collider2D.enabled = false;
			Destroy (gameObject, m_ParticleSystem.main.duration);
			// AudioManager.Singleton.PlayCoinSound (transform.position);
		}
	}
}