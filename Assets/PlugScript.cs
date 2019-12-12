using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugScript : MonoBehaviour
{
    private FixedJoint2D fixedJoint;
    private DistanceJoint2D distanceJoint;

    private bool readyToConnect = false;
    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // void 

    void OnTriggerEnter2D(Collider2D collidedObj)
    {
        print(collidedObj.tag);
        if (collidedObj.tag == "OutletTrigger")
        {
            print("PLUG CONNECTED");
            // Connect the plug
            // fixedJoint: plug-outlet
            fixedJoint.connectedBody = collidedObj.gameObject.GetComponentInParent<Rigidbody2D>();
            fixedJoint.enabled = true;
        }
        else if (collidedObj.tag == "Outlet")
        {
            print("Outlet trigger");
            readyToConnect = true;
        }
    }

    public void OnReadyToConnect ()
    {
        // distanceJoint: plug-player
        if (readyToConnect)
            distanceJoint.enabled = false;
    }

    public void OnDisconnect ()
    {
        fixedJoint.enabled = false;
        distanceJoint.enabled = true;
        readyToConnect = false;
    }

}
