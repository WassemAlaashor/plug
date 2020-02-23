using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : MonoBehaviour
{

    [SerializeField]
    private float m_CatchUpSpeed = 2.0f;
    private Vector3 m_RelativePos;
    private Transform m_Target;
    private Transform m_Transform;

    void Start()
    {
        m_Transform = transform; // cache camera transform
        m_RelativePos = m_Transform.localPosition; // get initial relative position
        m_Target = m_Transform.parent; // our target is the current parent (the player)
        m_Transform.parent = null; // unchild - it will move by itself
    }

    void LateUpdate()
    {
        // find the destination position in world space:
        Vector3 destPos = m_Target.TransformPoint(m_RelativePos);
        // lerp to it each frame:
        m_Transform.position = Vector3.Lerp(m_Transform.position, destPos, m_CatchUpSpeed * Time.deltaTime);
        // keep your eyes on the target
        m_Transform.LookAt(m_Target);
    }
}
