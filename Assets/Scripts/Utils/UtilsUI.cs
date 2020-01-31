using System;
using UnityEngine.UI;

namespace PlugBoy.UI
{
    public static class UtilsUI
    {
        public static void SetButtonAction(this Button button, Action action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action());
        }
    }
}