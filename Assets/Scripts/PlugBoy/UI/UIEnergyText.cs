using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Collectables;
using System;

namespace PlugBoy.UI
{
    public class UIEnergyText : UIText
    {
        [SerializeField]
        protected string m_EnergyTextFormat = "% {0}";

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            GameManager.Singleton.Player.CurrentEnergy.AddEventAndFire(UpdateEnergyText, this);
        }

        private void UpdateEnergyText(float newEnergyValue)
        {
            float correctedValue = Mathf.Clamp(Mathf.Floor(newEnergyValue), 0, 100);
            text = string.Format(m_EnergyTextFormat, correctedValue);
        }
    }
}