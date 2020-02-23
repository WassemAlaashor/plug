using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugBoy.Utilities
{

	public class RenderDelay : MonoBehaviour
	{
        [SerializeField]
        private float m_RenderDelay;
        [SerializeField]
        private SpriteRenderer m_Renderer;
        [SerializeField]
        private Collider2D m_Collider;
        IEnumerator _Wait()
        {
            m_Renderer.enabled = false;
            if (m_Collider)
                m_Collider.enabled = false;
            yield return new WaitForSeconds(m_RenderDelay);
            m_Renderer.enabled = true;
            if (m_Collider)
                m_Collider.enabled = true;
        }

        void Start()
        {
            StartCoroutine(_Wait());
        }
	}
}