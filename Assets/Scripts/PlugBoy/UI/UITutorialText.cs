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
        protected bool m_Shown = false;
        protected string m_TutorialTitle;
        protected string m_TutorialText;

        IEnumerator _Show()
        {
            m_Animator.SetBool("Opened", true);
            yield return new WaitForSecondsRealtime(m_Duration);
            m_Animator.SetBool("Opened", false);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (m_Shown && m_OneShot) { return; }
            m_Shown = true;
            StartCoroutine(_Show());
        }
	}
}