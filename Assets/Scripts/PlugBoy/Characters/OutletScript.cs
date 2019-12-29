using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OutletScript : MonoBehaviour
{
    
    private bool forceActive = false;
    private PointEffector2D pointEffector;
    private CircleCollider2D outletTrigger;

    // Start is called before the first frame update
    void Start()
    {
        pointEffector = GetComponent<PointEffector2D>();
        outletTrigger = GameObject.Find("Trigger").GetComponent<CircleCollider2D>();
        pointEffector.enabled = forceActive;
        outletTrigger.enabled = forceActive;
    }

    // Update is called once per frame
    void Update()
    {
        pointEffector.enabled = forceActive;
        outletTrigger.enabled = forceActive;
    }

    public void Enable ()
    {
        print("FORCE ACTIVATED");
        forceActive = true;
    }

    
    public void Disable ()
    {
        forceActive = false;
    }

}
