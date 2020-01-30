using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlugBoy.UI
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField]
        private Slider m_Slider;

        void Update()
        {
            if (m_Slider)
            {
                m_Slider.value = GameManager.Singleton.CurrentEnergy.Value;
            }
        }
    }

}
