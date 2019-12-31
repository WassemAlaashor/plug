using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using PlugBoy.Utilities;

namespace PlugBoy.Characters
{

    public class PlugBoyCharacter : Character
    {
        #region Fields

        [Header("Character Details")]
        [Space]
        [SerializeField]
        protected float m_MaxRunSpeed = 8f;
        [SerializeField]
        protected float m_RunSmoothTime = 5f;
        [SerializeField]
        protected float m_RunSpeed = 5f;
        [SerializeField]
        protected float m_WalkSpeed = 1.75f;
        [SerializeField]
        protected float m_JumpStrength = 10f;
        [SerializeField]
        protected string[] m_Actions = new string[0];
        [SerializeField]
        protected int m_CurrentActionIndex = 0;

        [Header("Character Reference")]
        [Space]
        [SerializeField]
        protected Rigidbody2D m_Rigidbody2D;
        [SerializeField]
        protected Collider2D m_Collider2D;
        [SerializeField]
        protected Animator m_Animator;
        [SerializeField]
        protected GroundCheck m_GroundCheck;
        [SerializeField]
        protected ParticleSystem m_RunParticleSystem;
        [SerializeField]
        protected ParticleSystem m_JumpParticleSystem;
        [SerializeField]
        protected ParticleSystem m_WaterParticleSystem;
        [SerializeField]
        protected ParticleSystem m_BloodParticleSystem;
        [SerializeField]
        protected Skeleton m_Skeleton;
        [SerializeField]
        protected Plug m_Plug;

        [Header("Character Audio")]
        [Space]
        [SerializeField]
        protected AudioSource m_MainAudioSource;
        [SerializeField]
        protected AudioSource m_FootstepAudioSource;
        [SerializeField]
        protected AudioSource m_JumpAndGroundedAudioSource;

        #endregion

        #region Private Variables

        protected int m_CurrentHP = 1000;
        protected bool m_ClosingEye = false;
        protected Vector2 m_Speed = Vector2.zero;
        protected bool m_Moving = false;
        protected float m_CurrentRunSpeed = 0f;
        protected float m_SprintMultiplier = 2f;
        protected float m_CurrentSmoothVelocity = 0f;
        protected int m_CurrentFootstepSoundIndex = 0;
        protected Vector3 m_InitialScale;
        protected Vector3 m_InitialPosition;

        #endregion

        #region Properties

        public override float MaxRunSpeed
        {
            get
            {
                return m_MaxRunSpeed;
            }
        }

        public override float RunSmoothTime
        {
            get
            {
                return m_RunSmoothTime;
            }
        }

        public override float RunSpeed
        {
            get
            {
                return m_RunSpeed;
            }
        }

        public override float WalkSpeed
        {
            get
            {
                return m_WalkSpeed;
            }
        }

        public override float JumpStrength
        {
            get
            {
                return m_JumpStrength;
            }
        }

        public override Vector2 Speed
        {
            get
            {
                return m_Speed;
            }
        }

        public override string[] Actions
        {
            get
            {
                return m_Actions;
            }
        }

        public override string CurrentAction
        {
            get
            {
                return m_Actions[m_CurrentActionIndex];
            }
        }

        public override int CurrentActionIndex
        {
            get
            {
                return m_CurrentActionIndex;
            }
        }

        public override GroundCheck GroundCheck
        {
            get
            {
                return m_GroundCheck;
            }
        }

        public override Rigidbody2D Rigidbody2D
        {
            get
            {
                return m_Rigidbody2D;
            }
        }

        public override Collider2D Collider2D
        {
            get
            {
                return m_Collider2D;
            }
        }

        public override Animator Animator
        {
            get
            {
                return m_Animator;
            }
        }

        public override ParticleSystem RunParticleSystem
        {
            get
            {
                return m_RunParticleSystem;
            }
        }

        public override ParticleSystem JumpParticleSystem
        {
            get
            {
                return m_JumpParticleSystem;
            }
        }

        public override ParticleSystem WaterParticleSystem
        {
            get
            {
                return m_WaterParticleSystem;
            }
        }

        public override ParticleSystem BloodParticleSystem
        {
            get
            {
                return m_BloodParticleSystem;
            }
        }

        public override Skeleton Skeleton
        {
            get
            {
                return m_Skeleton;
            }
        }

        public override bool ClosingEye
        {
            get
            {
                return m_ClosingEye;
            }
        }

        public override AudioSource Audio
        {
            get
            {
                return m_MainAudioSource;
            }
        }

        #endregion

        #region MonoBehaviour Messages

        void Awake()
        {
            m_InitialPosition = transform.position;
            m_InitialScale = transform.localScale;
            m_GroundCheck.OnGrounded += GroundCheck_OnGrounded;
            m_Skeleton.OnActiveChanged += Skeleton_OnActiveChanged;
            m_Plug.OnPlugConnected += Plug_OnPlugConnected;
            IsDead = new Property<bool>(false);
            m_ClosingEye = false;
            m_CurrentFootstepSoundIndex = 0;
            GameManager.OnReset += GameManager_OnReset;
        }

        void Update()
        {
            if (!GameManager.Singleton.gameStarted || !GameManager.Singleton.gameRunning)
            {
                return;
            }

            if (transform.position.y < -2f || m_CurrentHP < 1)
            {
                Die();
            }

            // Speed
            m_Speed = new Vector2(Mathf.Abs(m_Rigidbody2D.velocity.x), Mathf.Abs(m_Rigidbody2D.velocity.y));

            // Speed Calculations

            m_CurrentRunSpeed = m_RunSpeed;
            if (m_Speed.x >= m_RunSpeed)
            {
                m_CurrentRunSpeed = Mathf.SmoothDamp(m_Speed.x, m_MaxRunSpeed, ref m_CurrentSmoothVelocity, m_RunSmoothTime);
            }

            if (Input.GetButtonDown("Sprint"))
            {
                m_CurrentRunSpeed *= m_SprintMultiplier;
                print("SHIFT DOWN");
            }
            if (Input.GetButtonUp("Sprint"))
            {
                m_CurrentRunSpeed /= m_SprintMultiplier;
                print("SHIFT UP");
            }

            Move(Input.GetAxis("Horizontal"));
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if (IsDead.Value && !m_ClosingEye)
            {
                StartCoroutine(CloseEye());
            }
        }

        void LateUpdate()
        {
            m_Animator.SetFloat("Speed", m_Speed.x);
            m_Animator.SetFloat("VelocityX", Mathf.Abs(m_Rigidbody2D.velocity.x));
            m_Animator.SetFloat("VelocityY", m_Rigidbody2D.velocity.y);
            m_Animator.SetBool("IsGrounded", m_GroundCheck.IsGrounded);
            m_Animator.SetBool("IsDead", IsDead.Value);
        }

        void OnTriggerEnter2D(Collider2D collidedObj)
        {
            // Outlet collider
            if (collidedObj.tag == "Outlet" && collidedObj.usedByEffector == false)
            {
                print("CHARACTER: Entered outlet collider");
                // Activate the outlet
                Outlet outlet = collidedObj.gameObject.GetComponent<Outlet>();
                outlet.ForceActive = true;
                // Activate the plug
                // m_Plug.OnReadyToConnect();
            }
        }

        void OnTriggerExit2D(Collider2D collidedObj)
        {
            // Outlet force collider (outer collider)
            if (collidedObj.tag == "Outlet" && collidedObj.usedByEffector == true)
            {
                print("CHARACTER: Exited force collider");
                Outlet outlet = collidedObj.gameObject.GetComponent<Outlet>();
                outlet.ForceActive = false;
                m_Plug.DisconnectFromOutlet();
                m_Plug.AttachToPlayer();
            }
        }

        #endregion

        #region Private Methods

        IEnumerator CloseEye()
        {
            m_ClosingEye = true;
            yield return new WaitForSeconds(0.6f);
            while (m_Skeleton.RightEye.localScale.y > 0f)
            {
                if (m_Skeleton.RightEye.localScale.y > 0f)
                {
                    Vector3 scale = m_Skeleton.RightEye.localScale;
                    scale.y -= 0.1f;
                    m_Skeleton.RightEye.localScale = scale;
                }
                if (m_Skeleton.LeftEye.localScale.y > 0f)
                {
                    Vector3 scale = m_Skeleton.LeftEye.localScale;
                    scale.y -= 0.1f;
                    m_Skeleton.LeftEye.localScale = scale;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        #endregion

        #region Public Methods

        public virtual void PlayFootstepSound()
        {
            if (m_GroundCheck.IsGrounded)
            {
                // AudioManager.Singleton.PlayFootstepSound ( m_FootstepAudioSource, ref m_CurrentFootstepSoundIndex );
            }
        }

        public override void Move(float horizontalAxis)
        {
            if (!IsDead.Value)
            {
                float speed = m_CurrentRunSpeed;

                Vector2 velocity = m_Rigidbody2D.velocity;
                velocity.x = speed * horizontalAxis;
                m_Rigidbody2D.velocity = velocity;

                if (horizontalAxis > 0f)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Sign(horizontalAxis);
                    transform.localScale = scale;
                }
                else if (horizontalAxis < 0f)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Sign(horizontalAxis);
                    transform.localScale = scale;
                }

                if (m_Plug.Connected)
                {
                    // Move input check for RunParticle edge case
                    if (horizontalAxis != 0)
                    {
                        m_Moving = true;
                    }
                    else
                    {
                        m_Moving = false;
                    }
                }
            }
        }

        public override void Jump()
        {
            if (!IsDead.Value)
            {
                if (m_GroundCheck.IsGrounded)
                {
                    Vector2 velocity = m_Rigidbody2D.velocity;
                    velocity.y = m_JumpStrength;
                    m_Rigidbody2D.velocity = velocity;
                    m_Animator.ResetTrigger("Jump");
                    m_Animator.SetTrigger("Jump");
                    m_JumpParticleSystem.Play();
                    // AudioManager.Singleton.PlayJumpSound ( m_JumpAndGroundedAudioSource );
                }
            }
        }

        public override void Die()
        {
            Die(false);
        }

        public override void Die(bool blood)
        {
            if (!IsDead.Value)
            {
                IsDead.Value = true;
                m_Skeleton.SetActive(true, m_Rigidbody2D.velocity);
                if (blood)
                {
                    ParticleSystem particle = Instantiate<ParticleSystem>(
                                                  m_BloodParticleSystem,
                                                  transform.position,
                                                  Quaternion.identity);
                    Destroy(particle.gameObject, particle.main.duration);
                }
                // CameraController.Singleton.fastMove = true;
            }
        }

        public override void EmitRunParticle()
        {
            if (!IsDead.Value)
            {
                // Physics of the joint between plug and character can trigger
                // To avoid that, we're checking if we have movement input when the joint is active
                if (m_Plug.Connected && !m_Moving)
                {
                    return;
                }
                else
                {
                    m_RunParticleSystem.Emit(1);
                }
            }
        }

        public override void Reset()
        {
            IsDead.Value = false;
            m_CurrentHP = 1000;
            m_ClosingEye = false;
            m_CurrentFootstepSoundIndex = 0;
            transform.position = m_InitialPosition;
            transform.localScale = m_InitialScale;
            m_Plug.transform.position = m_InitialPosition; // TODO: Custom respawn for plug
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Skeleton.SetActive(false, m_Rigidbody2D.velocity);
        }

        #endregion

        #region Events

        void GameManager_OnReset()
        {
            Reset();
        }

        void Skeleton_OnActiveChanged(bool active)
        {
            m_Animator.enabled = !active;
            m_Collider2D.enabled = !active;
            m_Rigidbody2D.simulated = !active;
        }

        void GroundCheck_OnGrounded()
        {
            if (!IsDead.Value)
            {
                m_JumpParticleSystem.Play();
                // AudioManager.Singleton.PlayGroundedSound ( m_JumpAndGroundedAudioSource );
            }
        }

        void Plug_OnPlugConnected()
        {
            print("CHARACTER: PLUG CONNECTED");
        }

        #endregion

        [System.Serializable]
        public class CharacterDeadEvent : UnityEvent
        {

        }

    }

}