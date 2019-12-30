using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Outlet : MonoBehaviour
{

    [SerializeField]
    protected GameObject m_OutletTrigger;
    protected bool m_ForceActive = false;
    protected bool m_PlugConnected = false;
    protected PointEffector2D m_PointEffector;
    protected CircleCollider2D m_ConnectTrigger;
    protected CircleCollider2D m_ForceCollider;

    public virtual bool ForceActive {
        get {
            return m_ForceActive;
        }
        set {
            m_ForceActive = value;
        }
    }

    public virtual bool PlugConnected {
        get {
            return m_PlugConnected;
        }
        set {
            m_PlugConnected = value;
        }
    }

    void Start()
    {
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

    void Update()
    {
        m_ForceCollider.enabled = m_ForceActive;
        m_PointEffector.enabled = m_ForceActive;
        m_ConnectTrigger.enabled = m_ForceActive;
    }

}
