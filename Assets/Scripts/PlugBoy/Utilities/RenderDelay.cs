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
        IEnumerator _Wait()
        {
            m_Renderer.enabled = false;
            yield return new WaitForSeconds(m_RenderDelay);
            m_Renderer.enabled = true;
        }

        void Start()
        {
            StartCoroutine(_Wait());
        }
	}
}