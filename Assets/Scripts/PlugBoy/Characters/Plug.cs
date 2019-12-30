using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlugBoy.Characters
{

    public class Plug : MonoBehaviour
    {

        // [SerializeField]
        // protected Cable m_Cable;

        [SerializeField]
        protected Transform m_PlugEndPoint;

        protected SpriteRenderer m_SpriteDisconnected;
        protected SpriteRenderer m_SpriteConnected;
        protected Rigidbody2D m_RigidBody;
        protected DistanceJoint2D m_DistanceJoint;
        protected Vector3 m_PlugEndPointInitialLocalPosition;
        protected bool m_Connected;
        protected Outlet m_ConnectedOutlet;
        public virtual bool Connected {
            get {
                return m_Connected;
            }
        }
        public delegate void PlugConnectedHandler();
        public event PlugConnectedHandler OnPlugConnected;
        // public event PlugConnectedHandler OnPlugDisconnected;


        void Start()
        {
            m_SpriteDisconnected = GetComponent<SpriteRenderer>();
            m_SpriteConnected = GameObject.Find("Plug Connected Sprite").GetComponent<SpriteRenderer>();
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_DistanceJoint = GetComponent<DistanceJoint2D>();
            m_PlugEndPointInitialLocalPosition = m_PlugEndPoint.localPosition;
        }

        void Update()
        {
            // Angle correction
            // transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, m_Cable.GetTailAngle() + 90f);
        }

        void OnTriggerEnter2D(Collider2D collidedObj)
        {
            if (collidedObj.tag == "OutletTrigger")
            {
                // Connect the plug
                // fixedJoint: plug-outlet
                ConnectToOutlet(collidedObj.gameObject);
            }
            // else if (collidedObj.tag == "Outlet")
            // {
            //     print("PLUG: OUTER COLLIDER");
            // }
        }

        void ConnectToOutlet(GameObject collidedOutlet)
        {
            transform.position = collidedOutlet.transform.position;
            transform.rotation = Quaternion.identity;
            m_RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            m_SpriteDisconnected.enabled = false;
            m_SpriteConnected.enabled = true;
            m_PlugEndPoint.localPosition = new Vector3(0, 0, 0);
            m_Connected = true;
            m_ConnectedOutlet = collidedOutlet.transform.parent.GetComponent<Outlet>();
            m_ConnectedOutlet.PlugConnected = true;
            OnPlugConnected(); // Fire event
        }

        public virtual void DisconnectFromOutlet()
        {
            m_RigidBody.constraints = RigidbodyConstraints2D.None;
            m_SpriteDisconnected.enabled = true;
            m_SpriteConnected.enabled = false;
            m_PlugEndPoint.localPosition = m_PlugEndPointInitialLocalPosition;
            m_Connected = false;
            m_ConnectedOutlet.PlugConnected = false;
            // OnPlugDisconnected(); // Fire event
        }

        public virtual void AttachToPlayer()
        {
            m_DistanceJoint.enabled = true;
        }

        public virtual void DetachFromPlayer()
        {
            m_DistanceJoint.enabled = false;
        }
    }

}
