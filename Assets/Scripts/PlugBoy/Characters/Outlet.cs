using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Outlet : MonoBehaviour
{

    [SerializeField]
    private GameObject outletTrigger;
    private bool forceActive = false;
    private PointEffector2D pointEffector;
    private CircleCollider2D connectTrigger;
    private CircleCollider2D forceCollidder;

    void Start()
    {
        pointEffector = GetComponent<PointEffector2D>();
        connectTrigger = outletTrigger.GetComponent<CircleCollider2D>();
        // Finding force collider (outer collider)
        foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
        {
            if (collider.usedByEffector)
            {
                forceCollidder = collider;
            }
        }
        forceCollidder.enabled = forceActive;
        pointEffector.enabled = forceActive;
        connectTrigger.enabled = forceActive;
    }

    void Update()
    {
        forceCollidder.enabled = forceActive;
        pointEffector.enabled = forceActive;
        connectTrigger.enabled = forceActive;
        // GetComponent<SpriteRenderer>().enabled = forceActive;
    }

    public void Enable()
    {
        // Point effector, force collider and trigger collider
        forceActive = true;
        print("Outlet Enabled");
    }


    public void Disable()
    {
        forceActive = false;
    }

}
