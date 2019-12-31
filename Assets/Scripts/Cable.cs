using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField] protected Transform m_StartPoint;

    [SerializeField] protected Transform m_EndPoint;

    protected LineRenderer m_LineRenderer;
    protected List<CableSegment> m_CableSegments = new List<CableSegment>();
    public float m_CableSegmentLength = 0.1f; // 0.25f;
    public int m_CableSegmentCount = 20;
    public float m_LineWidth = 0.1f;

    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        Vector3 cableStartPoint = m_StartPoint.position;

        for (int i = 0; i < m_CableSegmentCount; i++)
        {
            m_CableSegments.Add(new CableSegment(cableStartPoint));
            cableStartPoint.y -= m_CableSegmentLength;
        }
    }

    void Update() => DrawCable();

    void FixedUpdate() => Simulate();

    protected void Simulate()
    {
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < m_CableSegmentCount; i++)
        {
            CableSegment firstSegment = m_CableSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            m_CableSegments[i] = firstSegment;
        }

        for (int i = 0; i < 50; i++)
        {
            ApplyConstraint();
        }
    }

    protected void ApplyConstraint()
    {
        //Constraint to First Point 
        CableSegment firstSegment = m_CableSegments[0];
        firstSegment.posNow = m_StartPoint.position;
        m_CableSegments[0] = firstSegment;


        //Constraint to Second Point 
        CableSegment endSegment = m_CableSegments[m_CableSegments.Count - 1];
        endSegment.posNow = m_EndPoint.position;
        m_CableSegments[m_CableSegments.Count - 1] = endSegment;

        for (int i = 0; i < m_CableSegmentCount - 1; i++)
        {
            CableSegment firstSeg = m_CableSegments[i];
            CableSegment secondSeg = m_CableSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - m_CableSegmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > m_CableSegmentLength)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < m_CableSegmentLength)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                m_CableSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                m_CableSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                m_CableSegments[i + 1] = secondSeg;
            }
        }
    }

    protected void DrawCable()
    {
        // float m_LineWidth = m_LineWidth;
        m_LineRenderer.startWidth = m_LineWidth;
        m_LineRenderer.endWidth = m_LineWidth;

        Vector3[] cablePositions = new Vector3[m_CableSegmentCount];
        for (int i = 0; i < m_CableSegmentCount; i++)
        {
            cablePositions[i] = m_CableSegments[i].posNow;
        }

        m_LineRenderer.positionCount = cablePositions.Length;
        m_LineRenderer.SetPositions(cablePositions);
    }

    // public float GetTailAngle()
    // {
    //     CableSegment p1 = m_CableSegments[m_CableSegments.Count - 1];
    //     CableSegment p2 = m_CableSegments[m_CableSegments.Count - 2];
    //     return Mathf.Atan2(p2.posNow.y - p1.posNow.y, p2.posNow.x - p1.posNow.x) * Mathf.Rad2Deg;
    // }

    public struct CableSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public CableSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
