using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using KeySystem;

public class HorrorTrigger : MonoBehaviour
{
    public bool UseTriggerOnEnter;
    public bool UseTriggerOnExit;
    public bool DestroyAfterUse;

    private int doneEvents = 0;
    private int numberOfEvents = 0;
    private bool coroutineDone = false;

    private GameObject player;
    private Camera p_camera;
    private CharacterController CharacterController;
    private FirstPersonController playerFPSController;

    public bool disableAtStart;
    public AudioSource audioSource;
    private float fadeTime = 2f;

    [Header("---DISABLE MOVEMENT SETTINGS---")]
    public bool disablePlayer;
    public bool disablePlayerMovement;
    public float delayBeforeMovingAgain;
    public bool disablePlayerCrouch;


    [Header("---MOVE OBJECT SETTINGS---")]
    public bool move;
    public MovableObject movableObject;
    public float delayMoveObject;

    [Header("---TELEPORT OBJECT SETTINGS---")]
    public bool teleport;
    public MovableObject teleportObject;

    [Header("---LIGHTS ON/OFF SETTINGS---")]
    public bool Uselights;
    public bool lightsOn;
    public bool lightsOff;
    public bool lightFlicker;
    public float LightsDisableFor;

    public List<Light> LightObjs = new List<Light>();

    [Header("---JUMPSCARE SETTINGS---")]
    public bool UseJumpScare;
    public Image jumpSImage;
    public float stayOnScreenFor;

    [Header("---LOOK AT SETTINGS---")]
    public bool look;
    public Transform lookAt;
    public float lookAtDelay;
    public float damping;
    private Vector3 lookPos = new Vector3();
    private bool startLook;

    [Header("---DROP OBJECT SETTINGS---")]
    public bool drop;
    public List<FallObject> DropObjects = new List<FallObject>();

    [Header("---LEVITATE OBJECTS SETTINGS---")]
    public bool levitate;
    public List<FallObject> LevitateObjects = new List<FallObject>();
    public float forceUp;
    public float forceDown;
    public float delay;

    [Header("---THROW OBJECT SETTINGS---")]
    public bool UseThrow;
    //public FallObject throwObject;
    public List<FallObject> ThrowObjects = new List<FallObject>();
    public float force;

    [Header("---ENABLE/DISABLE OBJECTS---")]
    public bool enableObject;
    public bool disableObject;
    public GameObject otherObject;
    public float EnableFor;
    public float DisableAfter;
    public bool DisableTorch;

    public bool PlaySting;
    public string StingName;

    [Header("---ENABLE OTHER TRIGGER SETTINGS---")]
    public bool enableOtherTrigger;
    public HorrorTrigger otherTrigger;

    [Header("---DOOR SETTINGS---")]
    public bool UseDoor;
    public bool openDoor;
    public bool closeDoor;
    public bool slamDoor;
    public bool eventDoor;
    public bool lockDoor;
    public bool unlockDoor;
    public MyDoorController doorController;

    [Header("---SOUNDS SETTINGS---")]
    public bool sounds;
    public bool fadeIn;
    public bool fadeOut;
    public SoundEffectManager SoundEffectManager;
    public bool x;

    [Header("---PLAY SOUND---")]
    public bool play;
    public string clipName;

    [Header("---STOP SOUND---")]
    public bool stop;
    public string stopClipName;

    [Header("---ANIMATION SETTINGS---")]
    public bool AnimEnable;
    public bool PlayAnim;
    public bool StopAnim;
    public Animator Anim_1;



    public void Start()
    {
        if (disableAtStart) ActivateTriggerCollider(false);
        player = GameObject.FindWithTag("Player");
        p_camera = Camera.main;
        CharacterController = player.GetComponentInChildren<CharacterController>();
        playerFPSController = player.GetComponentInChildren<FirstPersonController>();
    }
    public void Update()
    {
        if (startLook)
        {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(lookPos),
                        Time.deltaTime * damping);
            p_camera.transform.localEulerAngles = new Vector3(Mathf.Lerp(p_camera.transform.localEulerAngles.x, 0, Time.deltaTime), 0, 0);
        }
        if (doneEvents == numberOfEvents && coroutineDone) GetComponent<BoxCollider>().enabled = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (UseTriggerOnEnter)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Trigger!");
                StartCoroutine(HelpCoroutine());
                if (disablePlayer)
                {
                    numberOfEvents++;
                    DisablePlayerMovement();
                }
                if (disablePlayerCrouch)
                {
                    playerFPSController.canCrouch = false;
                }
                if (move)
                {
                    numberOfEvents++;
                    Move();
                }
                if (teleport)
                {
                    numberOfEvents++;
                    Teleport();
                }
                if (Uselights)
                {
                    numberOfEvents++;
                    Lights();
                }
                if (UseJumpScare)
                {
                    numberOfEvents++;
                    Jumpscare();
                }
                if (look)
                {
                    numberOfEvents++;
                    LookAt();
                }
                if (sounds)
                {
                    numberOfEvents++;
                    Sounds();
                }
                if (drop)
                {
                    numberOfEvents++;
                    Drop();
                }
                if (levitate)
                {
                    numberOfEvents++;
                    Levitate();
                }
                if (UseThrow)
                {
                    numberOfEvents++;
                    ThrowObject(force);
                }
                if (enableOtherTrigger)
                {
                    numberOfEvents++;
                    otherTrigger.ActivateTriggerCollider(true);
                }
                if (enableObject)
                {
                    numberOfEvents++;
                    HideObject();
                }
                if (disableObject)
                {
                    numberOfEvents++;
                    RemoveObject();
                }
                if (UseDoor)
                {
                    numberOfEvents++;
                    InteractDoor();
                }
                if (AnimEnable)
                {
                    numberOfEvents++;
                    Animations();
                }
                if (DestroyAfterUse)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                }

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (UseTriggerOnExit)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Trigger!");
                StartCoroutine(HelpCoroutine());
                if (disablePlayer)
                {
                    numberOfEvents++;
                    DisablePlayerMovement();
                }
                if (disablePlayerCrouch)
                {
                    playerFPSController.canCrouch = true;
                }
                if (move)
                {
                    numberOfEvents++;
                    Move();
                }
                if (teleport)
                {
                    numberOfEvents++;
                    Teleport();
                }
                if (Uselights)
                {
                    numberOfEvents++;
                    Lights();
                }
                if (UseJumpScare)
                {
                    numberOfEvents++;
                    Jumpscare();
                }
                if (look)
                {
                    numberOfEvents++;
                    LookAt();
                }
                if (sounds)
                {
                    numberOfEvents++;
                    Sounds();
                }
                if (drop)
                {
                    numberOfEvents++;
                    Drop();
                }
                if (levitate)
                {
                    numberOfEvents++;
                    Levitate();
                }
                if (UseThrow)
                {
                    numberOfEvents++;
                    ThrowObject(force);
                }
                if (enableOtherTrigger)
                {
                    numberOfEvents++;
                    otherTrigger.ActivateTriggerCollider(true);
                }
                if (enableObject)
                {
                    numberOfEvents++;
                    HideObject();
                }
                if (disableObject)
                {
                    numberOfEvents++;
                    RemoveObject();
                }
                if (UseDoor)
                {
                    numberOfEvents++;
                    InteractDoor();
                }
                if (AnimEnable)
                {
                    numberOfEvents++;
                    Animations();
                }
                if (DestroyAfterUse)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
        } 
    }
    private IEnumerator HelpCoroutine()
    {
        yield return new WaitForSeconds(2);
        coroutineDone = true;
    }
    private IEnumerator PlayerMovementCoroutine(float Delay)
    {
        //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //FindObjectOfType<WalkingSound>().canMakeSound = false;
        //cc.enabled = false;
        yield return new WaitForSeconds(Delay);
        CharacterController.enabled = true;
        //player.GetComponent<playerMovement>().enabled = true;
        //FindObjectOfType<WalkingSound>().canMakeSound = true;
        doneEvents++;
    }
    private IEnumerator StopMoveObject(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        movableObject.SetIsMoving(false);
        if (movableObject.GetDisableAM()) movableObject.gameObject.SetActive(false);
        doneEvents++;
    }
    private IEnumerator JumpscareStayOnScreen(float time)
    {
        //Activates the image in Canvas and then disables it
        jumpSImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        jumpSImage.gameObject.SetActive(false);
        doneEvents++;
    }
    private IEnumerator LookAtCoroutine(float delay)
    {
        player.GetComponentInChildren<FirstPersonController>().enabled = false;
        //FindObjectOfType<WalkingSound>().canMakeSound = false;
        yield return new WaitForSeconds(delay);
        startLook = false;
        player.GetComponentInChildren<FirstPersonController>().enabled = true;
        //FindObjectOfType<WalkingSound>().canMakeSound = true;
        doneEvents++;
    }
    private IEnumerator HideObjectCoroutine(float time)
    {
        bool IsTorchOn = p_camera.GetComponentInChildren<Light>().enabled;

        if (DisableTorch)
        {
            p_camera.GetComponentInChildren<Light>().enabled = false;
        }

        otherObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        if (DisableTorch && IsTorchOn)
        {
            p_camera.GetComponentInChildren<Light>().enabled = true;
        }

        otherObject.gameObject.SetActive(false);
        PlaySound(StingName);
        doneEvents++;
    }
    private IEnumerator DisableObjectCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        otherObject.gameObject.SetActive(false);
    }
    private IEnumerator AudioThings()
    {
        yield return new WaitForSeconds(2);
    }
    private IEnumerator LightsOffOn(float time)
    {
        foreach (Light light in LightObjs)
        {
            light.enabled = true;

            yield return new WaitForSeconds(time);

            light.enabled = false;
        }
    }

    public void DisablePlayerMovement()
    {
        if (disablePlayerMovement)
        {
            CharacterController.enabled = false;
            StartCoroutine(PlayerMovementCoroutine(delayBeforeMovingAgain));
        }
}
    public void Move()
    {
        //Disable the playerMovement and reset the rigidbody velocity
        //Starts the coroutine
        movableObject.gameObject.SetActive(true);
        movableObject.SetIsMoving(true);
        StartCoroutine(StopMoveObject(delayMoveObject));

    }
    public void Teleport()
    {
        //Basically just telport the movable object
        //Design-vise make sure the player cannot see the transport
        teleportObject.Teleport();
        doneEvents++;
    }
    public void Lights()
    {
        foreach (Light light in LightObjs)
        {
            if (lightsOn)
            {
                light.enabled = true;
            }
            if (lightsOff)
            {
                light.enabled = false;
            }
            if (lightFlicker)
            {
                StartCoroutine(LightsOffOn(LightsDisableFor));
            }
        }
        doneEvents++;
    }
    public void Jumpscare()
    {
        audioSource.Play();
        StartCoroutine(JumpscareStayOnScreen(stayOnScreenFor));
    }
    public void LookAt()
    {
        StartCoroutine(LookAtCoroutine(lookAtDelay));
        startLook = true;
        lookPos = lookAt.position - player.transform.position;
        lookPos.y = 0;
    }
    public void ActivateTriggerCollider(bool value)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = value;
        doneEvents++;
    }
    public void PlaySound(string clipName)
    {
        SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);
        doneEvents++;
    }
    public void StopSound(string clipName)
    {
        SoundEffectManager.GlobalSFXManager.PauseSFX(clipName);
        doneEvents++;
    }
    public void Drop()
    {
        foreach (FallObject dropObject in DropObjects)
        {
            dropObject.GetComponent<Rigidbody>().useGravity = true;
        }
        //dropObject.GetComponent<Rigidbody>().useGravity = true;
        doneEvents++;
    }
    public void Levitate()
    {
        foreach (FallObject levitateObject in LevitateObjects)
        {
            levitateObject.Levitate(forceUp, forceDown, delay);
        }
        doneEvents++;
    }
    public void ThrowObject(float force)
    {
        foreach (FallObject obj in ThrowObjects)

        {
            obj.Fly(force);
        }
        doneEvents++;
    }
    public void TestFunction(string[] test)
    {

    }
    public void HideObject()
    {
        StartCoroutine(HideObjectCoroutine(EnableFor));
    }
    public void RemoveObject()
    {
        StartCoroutine(DisableObjectCoroutine(DisableAfter));
    }
    public void InteractDoor()
    {
        if (openDoor)
        {
            doorController.OpenDoor();
        }
        if (closeDoor)
        {
            doorController.OpenDoor();
        }
        if (slamDoor)
        {
            doorController.SlamDoor();
            //Debug.Log("Slam!");
        }
        if (eventDoor)
        {
            doorController.LockedDoor();
        }
        if (lockDoor)
        {
            doorController.doorLocked = true;
        }
        if (unlockDoor)
        {
            doorController.doorLocked = false;
        }
    }
    public void Sounds()
    {
        if (play)
        {

            //SoundEffectManager.ClipList
            PlaySound(clipName);
            //audioSource(clipName);
            //StartCoroutine(AudioHelper.FadeIn(audioSource, fadeTime));
        }
        if (stop)
        {
            StopSound(stopClipName);
            //StartCoroutine(AudioHelper.FadeOut(audioSource, fadeTime));
        }
        //StartCoroutine(AudioThings());
    }
    public void Animations()
    {
        if (PlayAnim)
        {

        }
        if (StopAnim)
        {

        }
    }
}