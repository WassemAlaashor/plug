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
            yield return StartCoroutine(_CloseAllDialogs());
            m_Animator.SetBool("Opened", true);
            yield return new WaitForSecondsRealtime(m_Duration);
            m_Animator.SetBool("Opened", false);
        }

        IEnumerator _CloseAllDialogs()
        {
            // Finds and closes every tutorial dialog
            // FIXME: A bad solution but it is a last minute bugfix
            bool closedAtLeastOne = false;
            float defaultDelay = 0f;
            foreach (GameObject dialog in GameObject.FindGameObjectsWithTag("TutorialDialog"))
            {
                Animator animator = dialog.GetComponent<Animator>();
                if (animator.GetBool("Opened"))
                {
                    animator.SetBool("Opened", false);
                    closedAtLeastOne = true;
                    break;
                }
            }
            if (closedAtLeastOne)
            {
                defaultDelay = 0.5f;
            }
            yield return new WaitForSecondsRealtime(defaultDelay); // Wait for fadeout animation
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