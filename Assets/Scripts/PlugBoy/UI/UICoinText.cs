using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Collectables;
using System;

namespace PlugBoy.UI
{
	public class UICoinText : UIText
	{
		[SerializeField]
		protected string m_CoinTextFormat = "{0}/{1}";
        
        protected int m_Current;

        protected int m_Max;

		protected override void Awake ()
		{
			base.Awake ();
		}

        protected override void Start()
        {
            GameManager.Singleton.m_Coin.AddEventAndFire(UpdateCoinsTextCurrent, this);
            GameManager.Singleton.m_MaxCoin.AddEventAndFire(UpdateCoinsTextMax, this);
        }

        private void UpdateCoinsTextCurrent(int newCoinValue)
        {
            GetComponent<Animator>().SetTrigger("Collect");
            m_Current = newCoinValue;
            text = BuildText();
        }

        private void UpdateCoinsTextMax(int newCoinValue)
        {
            m_Max = newCoinValue;
            text = BuildText();
        }

        private string BuildText()
        {
            return string.Format(m_CoinTextFormat, m_Current, m_Max);
        }
	}
}