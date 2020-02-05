using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PlugBoy
{
    public class StarBackground : MonoBehaviour
    {
        public int m_MaxStars = 100;
        public float m_StarSize = 0.1f;
        public float m_StarSizeRange = 0.5f;
        public float m_FieldWidth = 20f;
        public float m_FieldHeight = 25f;
        public float m_ParallaxFactor = 0f;
        public bool m_Colorize = false;

        float m_xOffset;
        float m_yOffset;

        ParticleSystem m_ParticleSystem;
        ParticleSystem.Particle[] m_Stars;
        Transform theCamera;


        void Awake()
        {
            theCamera = Camera.main.transform;
            m_Stars = new ParticleSystem.Particle[m_MaxStars];
            m_ParticleSystem = GetComponent<ParticleSystem>();

            Assert.IsNotNull(m_ParticleSystem, "Particle system missing from object!");

            m_xOffset = m_FieldWidth * 0.5f;  // Offset the coordinates to distribute the spread
            m_yOffset = m_FieldHeight * 0.5f; // around the object's center

            for (int i = 0; i < m_MaxStars; i++)
            {
                float randSize = Random.Range(1f - m_StarSizeRange, m_StarSizeRange + 1f); // Randomize star size within parameters
                float scaledColor = (true == m_Colorize) ? randSize - m_StarSizeRange : 1f; // If coloration is desired, color based on size

                m_Stars[i].position = GetRandomInRectangle(m_FieldWidth, m_FieldHeight) + transform.position;
                m_Stars[i].startSize = m_StarSize * randSize;
                m_Stars[i].startColor = new Color(1f, scaledColor, scaledColor, 1f);
            }
            m_ParticleSystem.SetParticles(m_Stars, m_Stars.Length); // Write data to the particle system
        }

        void Update()
        {
            for (int i = 0; i < m_MaxStars; i++)
            {
                Vector3 pos = m_Stars[i].position + transform.position;

                if (pos.x < (theCamera.position.x - m_xOffset))
                {
                    pos.x += m_FieldWidth;
                }
                else if (pos.x > (theCamera.position.x + m_xOffset))
                {
                    pos.x -= m_FieldWidth;
                }

                if (pos.y < (theCamera.position.y - m_yOffset))
                {
                    pos.y += m_FieldHeight;
                }
                else if (pos.y > (theCamera.position.y + m_yOffset))
                {
                    pos.y -= m_FieldHeight;
                }

                m_Stars[i].position = pos - transform.position;
            }
            m_ParticleSystem.SetParticles(m_Stars, m_Stars.Length);

            Vector3 newPos = theCamera.position * m_ParallaxFactor; // Caculate the position of the object
            newPos.z = 0; // Force Z-axis to zero, since we're in 2D
            transform.position = newPos;

        }

        // GetRandomInRectangle
        //----------------------------------------------------------
        // Get a random value within a certain rectangle area
        //
        Vector3 GetRandomInRectangle(float width, float height)
        {
            float x = Random.Range(0, width);
            float y = Random.Range(0, height);
            return new Vector3(x - m_xOffset, y - m_yOffset, 0);
        }

    }
}