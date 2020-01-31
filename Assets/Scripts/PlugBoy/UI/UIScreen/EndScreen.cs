using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlugBoy.UI
{
    public class EndScreen : UIScreen
    {
        [SerializeField]
        protected Button NextButton = null;
        [SerializeField]
        protected Button RestartButton = null;
        [SerializeField]
        protected Button HomeButton = null;
        [SerializeField]
        protected Button ExitButton = null;

        private void Start()
        {
            NextButton.SetButtonAction(() => LevelManager.Singleton.LoadNextLevel());
            RestartButton.SetButtonAction(() => LevelManager.Singleton.ResetLevel());
        }

        public override void UpdateScreenStatus(bool open)
        {
            base.UpdateScreenStatus(open);
        }
    }

}