using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;



namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;



        /// FOOTSTEP SOUNDS ///

        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.

        [SerializeField] private AudioClip[] Wood_FootstepSounds;
        [SerializeField] private AudioClip[] Tile_FootstepSounds;
        [SerializeField] private AudioClip[] Carpet_FootstepSounds;
        [SerializeField] private AudioClip[] Metal_FootstepSounds;
        [SerializeField] private AudioClip[] Dirt_FootstepSounds;

        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.

        [SerializeField] private AudioClip Wood_JumpSound;
        [SerializeField] private AudioClip Tile_JumpSound;
        [SerializeField] private AudioClip Carpet_JumpSound;
        [SerializeField] private AudioClip Metal_JumpSound;
        [SerializeField] private AudioClip Dirt_JumpSound;

        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        [SerializeField] private AudioClip Wood_LandSound;
        [SerializeField] private AudioClip Tile_LandSound;
        [SerializeField] private AudioClip Carpet_LandSound;
        [SerializeField] private AudioClip Metal_LandSound;
        [SerializeField] private AudioClip Dirt_LandSound;

        /// FOOTSTEP SOUNDS ///



        [SerializeField] public bool canLook; // stops the camera from moving.

        //[SerializeField] public bool isCrouching = false;
        
        Rigidbody m_Rigidbody;
        const float k_Half = 0.5f;
        //private CrouchTrigger crouchTrigger;


        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        public bool m_Jumping;
        private AudioSource m_AudioSource;

        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        //CapsuleCollider m_CharacterController;
        bool m_Crouching;

        private const string WoodTag = "WoodFloor";
        private const string TileTag = "TileFloor";
        private const string CarpetTag = "CarpetFloor";
        private const string MetalTag = "MetalFloor";
        private const string DirtTag = "DirtFloor";

        void Awake()
        {
            canLook = true;
        }

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.enabled = !m_AudioSource.enabled;

            StartCoroutine(AudioDelay());
           

            m_MouseLook.Init(transform , m_Camera.transform);

            //crouchTrigger = gameObject.GetComponent<CrouchTrigger>();

            m_Rigidbody = GetComponent<Rigidbody>();
            //m_CharacterController = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_CharacterController.height;
            m_CapsuleCenter = m_CharacterController.center;
        }


        // Update is called once per frame
        private void Update()
        {
            if (canLook)
            {
                RotateView();
            }

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;


            bool crouch = Input.GetKey(KeyCode.C);

            ScaleCapsuleForCrouching(crouch);
            PreventStandingInLowHeadroom();

            //Crouch();
        }


        private void PlayLandingSound()
        {
            RaycastHit Landhit;

            Physics.Raycast(transform.position, -transform.up, out Landhit);

            //m_AudioSource.clip = m_LandSound;
            //m_AudioSource.Play();
            //m_NextStep = m_StepCycle + .5f;

            if (Landhit.collider.CompareTag(WoodTag))
            {
                m_AudioSource.clip = Wood_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }

            if (Landhit.collider.CompareTag(TileTag))
            {
                m_AudioSource.clip = Tile_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }

            if (Landhit.collider.CompareTag(CarpetTag))
            {
                m_AudioSource.clip = Carpet_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }

            if (Landhit.collider.CompareTag(MetalTag))
            {
                m_AudioSource.clip = Metal_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }

            if (Landhit.collider.CompareTag(DirtTag))
            {
                m_AudioSource.clip = Dirt_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);



            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);
        }


        private void PlayJumpSound()
        {
            RaycastHit Jumphit;
            Physics.Raycast(transform.position, -transform.up, out Jumphit);

            //m_AudioSource.clip = m_JumpSound;
            //m_AudioSource.Play();

            if (Jumphit.collider.CompareTag(WoodTag))
            {
                m_AudioSource.clip = Wood_JumpSound;
                m_AudioSource.Play();
            }

            if (Jumphit.collider.CompareTag(TileTag))
            {
                m_AudioSource.clip = Tile_JumpSound;
                m_AudioSource.Play();
            }

            if (Jumphit.collider.CompareTag(CarpetTag))
            {
                m_AudioSource.clip = Carpet_JumpSound;
                m_AudioSource.Play();
            }

            if (Jumphit.collider.CompareTag(MetalTag))
            {
                m_AudioSource.clip = Metal_JumpSound;
                m_AudioSource.Play();
            }

            if (Jumphit.collider.CompareTag(DirtTag))
            {
                m_AudioSource.clip = Dirt_JumpSound;
                m_AudioSource.Play();
            }
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }

            RaycastHit Floorhit;

            Physics.Raycast(transform.position, -transform.up, out Floorhit);

            if (Floorhit.collider.CompareTag(WoodTag))
            {
                // pick & play a random footstep sound from the array,
                // excluding sound at index 0
                int a = Random.Range(1, Wood_FootstepSounds.Length);
                m_AudioSource.clip = Wood_FootstepSounds[a];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                Wood_FootstepSounds[a] = Wood_FootstepSounds[0];
                Wood_FootstepSounds[0] = m_AudioSource.clip;
            }

            if (Floorhit.collider.CompareTag(TileTag))
            {
                int b = Random.Range(1, Tile_FootstepSounds.Length);
                m_AudioSource.clip = Tile_FootstepSounds[b];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                Tile_FootstepSounds[b] = Tile_FootstepSounds[0];
                Tile_FootstepSounds[0] = m_AudioSource.clip;
            }

            if (Floorhit.collider.CompareTag(CarpetTag))
            {
                int c = Random.Range(1, Carpet_FootstepSounds.Length);
                m_AudioSource.clip = Carpet_FootstepSounds[c];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                Carpet_FootstepSounds[c] = Carpet_FootstepSounds[0];
                Carpet_FootstepSounds[0] = m_AudioSource.clip;
            }

            if (Floorhit.collider.CompareTag(MetalTag))
            {
                int d = Random.Range(1, Metal_FootstepSounds.Length);
                m_AudioSource.clip = Metal_FootstepSounds[d];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                Metal_FootstepSounds[d] = Metal_FootstepSounds[0];
                Metal_FootstepSounds[0] = m_AudioSource.clip;
            }

            if (Floorhit.collider.CompareTag(DirtTag))
            {
                int e = Random.Range(1, Dirt_FootstepSounds.Length);
                m_AudioSource.clip = Dirt_FootstepSounds[e];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                Dirt_FootstepSounds[e] = Dirt_FootstepSounds[0];
                Dirt_FootstepSounds[0] = m_AudioSource.clip;
            }

            else
            {
                Debug.Log("No Floor Here");
            }
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;


            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        //private void Crouch(bool crouch)
        //{

        //    ScaleCapsuleForCrouching(crouch);
        //    PreventStandingInLowHeadroom();
        //    ////Set the key for crouch
        //    //var crouchButton = Input.GetKey(KeyCode.LeftControl);


        //    //if (!isCrouching && Input.GetButtonDown("Crouch"))
        //    //{
        //    //    //Set player height to 0.5 when holding crouch key and center to 0.25
        //    //    m_CharacterController.height = 1f;
        //    //    //playerCollider.center = new Vector3(playerCollider.center.x, 0.25f, playerCollider.center.z);
        //    //    isCrouching = true;
        //    //}
        //    //else
        //    //    if (isCrouching && Input.GetButtonDown("Crouch") && crouchTrigger.isObjectAbove == false)
        //    //{
        //    //    //var cantStandUp = Physics.Raycast(transform.position, Vector3.up, 2f);

        //    //    //Checks if player can stand up
        //    //    //if(!cantStandUp)
        //    //    //{
        //    //    //Sets player height back to 2 and resets center back to 0
        //    //    playerCollider.height = 2.4f;
        //    //    //playerCollider.center = new Vector3(playerCollider.center.x, 0f, playerCollider.center.z);
        //    //    isCrouching = false;
        //    //    //}
        //    //}
        //}

        private void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_CharacterController.isGrounded && crouch)
            {
                if (m_Crouching) return;
                m_CharacterController.height = m_CharacterController.height / 2f;
                m_CharacterController.center = m_CharacterController.center / 2f;
                m_Crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_CharacterController.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_CharacterController.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_CharacterController.radius * k_Half, crouchRayLength))
                {
                    m_Crouching = true;
                    return;
                }
                m_CharacterController.height = m_CapsuleHeight;
                m_CharacterController.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }

        private void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_CharacterController.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_CharacterController.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_CharacterController.radius * k_Half, crouchRayLength))
                {
                    m_Crouching = true;
                }
            }
        }

        public IEnumerator AudioDelay()
        {
            

           yield return new WaitForSeconds(.5f);
            m_AudioSource.enabled = !m_AudioSource.enabled;
        }
    }
}