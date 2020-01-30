using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PlugBoy.UI
{
	public class UIScreen : MonoBehaviour
	{
        [SerializeField]
        internal UIScreenInfo ScreenInfo;
		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected CanvasGroup m_CanvasGroup;
        [SerializeField]
        protected Image m_BlurImage;
        [SerializeField]
        protected float m_BlurSize = 2.0f;

        public bool IsOpen { get; set; }

        public virtual void UpdateScreenStatus(bool open)
        {
            m_Animator.SetBool("Open", open);
            m_CanvasGroup.interactable = open;
            m_CanvasGroup.blocksRaycasts = open;
            IsOpen = open;
        }

        void Update()
        {
            if (m_BlurImage)
            {
                m_BlurImage.material.SetFloat("_Size", m_BlurSize);
            }
        }
	}

}