using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugBoy.Utilities
{
    public class StickyPlatform : MonoBehaviour
    {
        [SerializeField]
        private Transform PlayerRoot;
        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                print("Player stick.");
                PlayerRoot.SetParent(transform);
            }
        }
        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                PlayerRoot.SetParent(null);
            }
        }
    }
}