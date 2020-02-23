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

        protected Transform m_DefaultParentTransform;

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
        public virtual Outlet ConnectedOutlet {
            get {
                return m_ConnectedOutlet;
            }
        }
        // public delegate void PlugConnectedHandler();
        // public event PlugConnectedHandler OnPlugConnected;
        // public event PlugConnectedHandler OnPlugDisconnected;


        void Start()
        {
            m_SpriteDisconnected = GetComponent<SpriteRenderer>();
            m_SpriteConnected = GameObject.Find("Plug Connected Sprite").GetComponent<SpriteRenderer>(); // FIXME
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_DistanceJoint = GetComponent<DistanceJoint2D>();
            m_PlugEndPointInitialLocalPosition = m_PlugEndPoint.localPosition;
            m_DefaultParentTransform = transform.parent;
        }

        // void Update()
        // {
            // // Angle correction
            // transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, m_Cable.GetTailAngle() + 90f);
        // }

        void OnTriggerEnter2D(Collider2D collidedObj)
        {
            if (collidedObj.tag == "OutletTrigger")
            {
                // Connect the plug
                // fixedJoint: plug-outlet
                ConnectToOutlet(collidedObj.gameObject);
            }
        }

        protected void ConnectToOutlet(GameObject collidedOutlet)
        {
            transform.position = collidedOutlet.transform.position;
            transform.rotation = Quaternion.identity;
            m_RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            // FIXME
            var isNonStandardOutlet = collidedOutlet.transform.parent.Find("NO_SPRITE_WHEN_CONNECTED");
            if (isNonStandardOutlet == null)
            {
                // A last minute addition, bad solution
                // Needed for when the outlets are non-standard like in level 3
                m_SpriteConnected.enabled = true;
            }
            m_SpriteDisconnected.enabled = false;
            m_PlugEndPoint.localPosition = Vector3.zero;
            m_Connected = true;
            m_ConnectedOutlet = collidedOutlet.transform.parent.GetComponent<Outlet>();
            m_ConnectedOutlet.PlugConnected = true;
            
            // If the outlet moves in any way, the plug has to move with it
            // Easily producable with a moving platform and an outlet as a child
            transform.parent = m_ConnectedOutlet.transform;
            
            // OnPlugConnected(); // Fire event
        }

        public virtual void DisconnectFromOutlet()
        {
            m_RigidBody.constraints = RigidbodyConstraints2D.None;
            m_SpriteDisconnected.enabled = true;
            m_SpriteConnected.enabled = false;
            m_PlugEndPoint.localPosition = m_PlugEndPointInitialLocalPosition;
            m_Connected = false;
            m_ConnectedOutlet.PlugConnected = false;

            transform.parent = m_DefaultParentTransform;
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
