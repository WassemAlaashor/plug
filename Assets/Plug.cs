using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public delegate void PlugConnectedHandler ();
    public event PlugConnectedHandler OnPlugConnected;
    // public event PlugConnectedHandler OnPlugDisconnected;

    private SpriteRenderer spriteDisconnected;
    private SpriteRenderer spriteConnected;
    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint;

    [SerializeField]
    private Cable cable;

    [SerializeField]
    private Transform PlugEndPoint;
    private Vector3 plugEndPointInitialLocalPosition;


    // Start is called before the first frame update
    void Start()
    {
        spriteDisconnected = GetComponent<SpriteRenderer>();
        spriteConnected = GameObject.Find("Plug Connected Sprite").GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        plugEndPointInitialLocalPosition = PlugEndPoint.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Angle correction
        // transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, cable.GetTailAngle() + 90f);
    } 

    void OnTriggerEnter2D(Collider2D collidedObj)
    {
        if (collidedObj.tag == "OutletTrigger")
        {
            print("PLUG CONNECTED");
            // Connect the plug
            // fixedJoint: plug-outlet
            ConnectToOutlet(collidedObj.gameObject);
        }
        else if (collidedObj.tag == "Outlet")
        {
            print("PLUG: OUTER COLLIDER");
        }
    }

    void ConnectToOutlet(GameObject collidedOutlet)
    {
        transform.position = collidedOutlet.transform.position;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spriteDisconnected.enabled = false;
        spriteConnected.enabled = true;
        PlugEndPoint.localPosition = new Vector3(0, 0, 0);
        OnPlugConnected(); // Fire event
    }

    public void DisconnectFromOutlet()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        spriteDisconnected.enabled = true;
        spriteConnected.enabled = false;
        PlugEndPoint.localPosition = plugEndPointInitialLocalPosition;
        // OnPlugDisconnected(); // Fire event
    }

    public void AttachToPlayer()
    {
        distanceJoint.enabled = true;
    }

    public void DetachFromPlayer()
    {
        distanceJoint.enabled = false;
    }

}
