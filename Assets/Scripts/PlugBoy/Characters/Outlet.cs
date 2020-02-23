using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Outlet : MonoBehaviour
{

    [SerializeField]
    protected GameObject m_OutletTrigger;

    [SerializeField]
    protected bool m_Discharger = false;
    [SerializeField]
    protected float m_ChargeRate = 0; // Discharge if discharger
    [SerializeField]
    protected ParticleSystem m_ParticleSystem;
    [SerializeField]
    protected SpriteRenderer m_OutletPoleSpriteRenderer;
    [SerializeField]
    protected Sprite m_AlternativeSprite;
    protected bool m_AlternativeSpriteActive;
    [SerializeField]
    protected Sprite m_DefaultSprite;
    protected bool m_NonStandardOutlet;
    protected bool m_ForceActive = false;
    protected bool m_PlugConnected = false;
    protected PointEffector2D m_PointEffector;
    protected CircleCollider2D m_ConnectTrigger;
    protected CircleCollider2D m_ForceCollider;

    public virtual bool Discharger
    {
        get
        {
            return m_Discharger;
        }
    }

    public virtual float ChargeRate
    {
        get
        {
            return m_ChargeRate;
        }
    }
    public virtual bool ForceActive
    {
        get
        {
            return m_ForceActive;
        }
        set
        {
            m_ForceActive = value;
        }
    }

    public virtual bool PlugConnected
    {
        get
        {
            return m_PlugConnected;
        }
        set
        {
            if (m_ParticleSystem) // FIXME
            {
                if (value) {
                    m_ParticleSystem.Play();
                }
                else
                {
                    m_ParticleSystem.Stop();
                }
            }
            m_PlugConnected = value;
        }
    }

    void Start()
    {
        if (m_ParticleSystem)
        {
            m_ParticleSystem.Stop();
        }
        m_NonStandardOutlet = transform.Find("NO_SPRITE_WHEN_CONNECTED") != null;
        m_PointEffector = GetComponent<PointEffector2D>();
        m_ConnectTrigger = m_OutletTrigger.GetComponent<CircleCollider2D>();
        // Finding force collider (which is the outer collider)
        // FIXME
        foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
        {
            if (collider.usedByEffector)
            {
                m_ForceCollider = collider;
            }
        }
        m_ForceCollider.enabled = m_ForceActive;
        m_PointEffector.enabled = m_ForceActive;
        m_ConnectTrigger.enabled = m_ForceActive;
    }

    void SwapToAlternativeSprite(bool shouldSwap)
    {
        if (m_AlternativeSprite == null) { return; }
        if (m_AlternativeSpriteActive == shouldSwap) { return; }
        m_OutletPoleSpriteRenderer.sprite = shouldSwap ? m_AlternativeSprite : m_DefaultSprite;
        m_AlternativeSpriteActive = shouldSwap;
    }

    void Update()
    {
        m_ForceCollider.enabled = m_ForceActive;
        m_PointEffector.enabled = m_ForceActive;
        m_ConnectTrigger.enabled = m_ForceActive;

        // FIXME
        // Non-standard outlet sprite swap
        if (m_PlugConnected && m_NonStandardOutlet)
        {
            // Swap to alternative
            SwapToAlternativeSprite(true);
        }
        else if (!m_PlugConnected && m_NonStandardOutlet)
        {
            // Swap back to default
            SwapToAlternativeSprite(false);
        }
    }

}
