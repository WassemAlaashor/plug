using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField] private Transform StartPoint;

    [SerializeField] private Transform EndPoint;

    private LineRenderer lineRenderer;
    private List<CableSegment> cableSegments = new List<CableSegment>();
    public float cableSegLen = 0.1f; // 0.25f;
    public int segmentLength = 20;
    public float lineWidth = 0.1f;

    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 cableStartPoint = StartPoint.position;

        for (int i = 0; i < segmentLength; i++)
        {
            this.cableSegments.Add(new CableSegment(cableStartPoint));
            cableStartPoint.y -= cableSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawCable();
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            CableSegment firstSegment = this.cableSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.cableSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        CableSegment firstSegment = this.cableSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.cableSegments[0] = firstSegment;


        //Constrant to Second Point 
        CableSegment endSegment = this.cableSegments[this.cableSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.cableSegments[this.cableSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            CableSegment firstSeg = this.cableSegments[i];
            CableSegment secondSeg = this.cableSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.cableSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > cableSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < cableSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.cableSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.cableSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.cableSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawCable()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] cablePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            cablePositions[i] = this.cableSegments[i].posNow;
        }

        lineRenderer.positionCount = cablePositions.Length;
        lineRenderer.SetPositions(cablePositions);
    }

    public float GetTailAngle()
    {
        CableSegment p1 = this.cableSegments[this.cableSegments.Count - 1];
        CableSegment p2 = this.cableSegments[this.cableSegments.Count - 2];
        return Mathf.Atan2(p2.posNow.y - p1.posNow.y, p2.posNow.x - p1.posNow.x) * Mathf.Rad2Deg;
    }

    public struct CableSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public CableSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
