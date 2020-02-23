using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlugBoy.UI;
using System.Linq;

namespace PlugBoy
{
    public enum UIScreenInfo
    {
        LOADING_SCREEN,
        START_SCREEN,
        END_SCREEN,
        PAUSE_SCREEN,
        IN_GAME_SCREEN
    }

    public class UIManager : MonoBehaviour
    {

        private static UIManager m_Singleton;

        public static UIManager Singleton
        {
            get
            {
                return m_Singleton;
            }
        }

        [SerializeField]
        private List<UIScreen> m_Screens;
        private UIScreen m_ActiveScreen;
        [SerializeField]
        private Texture2D m_CursorDefaultTexture;
        [SerializeField]
        private Texture2D m_CursorClickTexture;
        [SerializeField]
        private float m_CursorHideDelay = 1f;
        private Coroutine m_HideCursor;

        public List<UIScreen> UISCREENS
        {
            get
            {
                return m_Screens;
            }
        }

        public UIScreen GetUIScreen(UIScreenInfo screenInfo)
        {
            return m_Screens.Find(el => el.ScreenInfo == screenInfo);
        }

        void Awake()
        {
            if (m_Singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            m_Singleton = this;
            Cursor.SetCursor(m_CursorDefaultTexture, Vector2.zero, CursorMode.Auto);
        }


        public void Init()
        {
            CloseAllScreens();
        }

        void Update()
        {

            // Cursor texture
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(m_CursorClickTexture, Vector2.zero, CursorMode.Auto);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Cursor.SetCursor(m_CursorDefaultTexture, Vector2.zero, CursorMode.Auto);
            }

            // Hide cursor if inactive
            // FIXME: Memory safe?
            if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
            {
                if (m_HideCursor == null)
                {
                    m_HideCursor = StartCoroutine(HideCursor());
                }
            }
            else // Movement
            {
                if (m_HideCursor != null)
                {
                    StopCoroutine(m_HideCursor);
                    m_HideCursor = null;
                    Cursor.visible = true;
                }
            }
        }

        private IEnumerator HideCursor()
        {
            yield return new WaitForSecondsRealtime(m_CursorHideDelay);
            Cursor.visible = false;
        }

        public void OpenScreen(UIScreen screen)
        {
            CloseAllScreens();
            screen.UpdateScreenStatus(true);
            m_ActiveScreen = screen;
        }

        public void CloseScreen(UIScreen screen)
        {
            if (m_ActiveScreen == screen)
            {
                m_ActiveScreen = null;
            }
            screen.UpdateScreenStatus(false);
        }

        public void CloseAllScreens()
        {
            foreach (var screen in m_Screens)
                CloseScreen(screen);
        }

        bool IsAsScreenOpen()
        {
            foreach (var screen in m_Screens)
            {
                if (screen.IsOpen)
                    return true;
            }

            return false;
        }
    }

}