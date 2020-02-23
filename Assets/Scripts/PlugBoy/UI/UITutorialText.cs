using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Collectables;
using System;

namespace PlugBoy.UI
{
	public class UITutorialText : MonoBehaviour
	{
        [SerializeField]
        protected Animator m_Animator;
        [SerializeField]
        protected UIText m_Title;
        [SerializeField]
        protected UIText m_Text;
        [SerializeField]
        protected bool m_OneShot = true;
        [SerializeField]
        protected float m_Duration = 3.0f;
        [SerializeField]
        private bool m_IfLevelCanEnd = false;
        [SerializeField]
        private bool m_IfLevelCannotEnd = false;
        protected bool m_Shown = false;
        protected string m_TutorialTitle;
        protected string m_TutorialText;

        void Awake()
        {
            StartCoroutine(_StartupDelay()); // TODO: Check.
        }

        IEnumerator _StartupDelay()
        {
            enabled = false;
            yield return new WaitForSecondsRealtime(0.2f);
            enabled = true;
        }

        IEnumerator _Show()
        {
            m_Animator.SetBool("Opened", true);
            yield return new WaitForSecondsRealtime(m_Duration);
            m_Animator.SetBool("Opened", false);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (m_IfLevelCanEnd && !LevelCanEnd()) { return; }
            if(m_IfLevelCannotEnd && LevelCanEnd()) { return; }
            if (m_Shown && m_OneShot) { return; }
            m_Shown = true;
            StartCoroutine(_Show());
        }

        protected bool LevelCanEnd() => GameManager.Singleton.m_Coin.Value >= GameManager.Singleton.m_MaxCoin.Value;

	}
}