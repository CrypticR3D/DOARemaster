using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using KeySystem;

public class HorrorTrigger : MonoBehaviour
{
    private int doneEvents = 0;
    private int numberOfEvents = 0;
    private bool coroutineDone = false;
    public CharacterController cc;


    private GameObject player;
    //Rotating camera to the mirror settings
    public GameObject camera;

    public bool disableAtStart;
    public AudioSource audioSource;
    private float fadeTime = 2f;

    [Header("---DISABLE MOVEMENT SETTINGS---")]
    public bool disablePlayerMovement;
    public float delayBeforeMovingAgain;

    [Header("---MOVE OBJECT SETTINGS---")]
    public bool move;
    public MovableObject movableObject;
    public float delayMoveObject;

    [Header("---TELEPORT OBJECT SETTINGS---")]
    public bool teleport;
    public MovableObject teleportObject;

    [Header("---LIGHTS ON/OFF SETTINGS---")]
    public bool lights;
    public bool lightsOnOff;
    public List<Light> Lights = new List<Light>();

    [Header("---JUMPSCARE SETTINGS---")]
    public bool jump;
    public Image jumpSImage;
    public float stayOnScreenFor;

    [Header("---LOOK AT SETTINGS---")]
    public bool look;
    public Transform lookAt;
    public float lookAtDelay;
    public float damping;
    private Vector3 lookPos = new Vector3();
    private bool startLook;

    [Header("---PLAY SOUND SETTINGS---")]
    public bool play;
    public string clipName;

    [Header("---STOP SOUND SETTINGS---")]
    public bool stop;
    public string stopClipName;

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
    public bool throW;
    //public FallObject throwObject;
    public List<FallObject> ThrowObjects = new List<FallObject>();
    public float force;

    [Header("---ENABLE/DISABLE OBJECTS---")]
    public bool enableObject;
    public GameObject otherObject;
    public float EnableFor;
    public bool DisableTorch;

    [Header("---ENABLE OTHER TRIGGER SETTINGS---")]
    public bool enableOtherTrigger;
    public HorrorTrigger otherTrigger;

    [Header("---DOOR SETTINGS---")]
    public bool openDoor;
    public bool interactDoor;
    public MyDoorController doorController;

    [Header("---SOUNDS SETTINGS---")]
    public bool sounds;
    public bool fadeIn;
    public bool fadeOut;

    public AudioClip[] audioClipArray;

    public void Start()
    {
        if (disableAtStart) ActivateTriggerCollider(false);
        player = GameObject.FindWithTag("Player");
        camera = player.GetComponentInChildren<Camera>().gameObject;
        cc = player.GetComponentInChildren<CharacterController>();
    }
    public void Update()
    {
        if (startLook)
        {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(lookPos),
                        Time.deltaTime * damping);
            camera.transform.localEulerAngles = new Vector3(Mathf.Lerp(camera.transform.localEulerAngles.x, 0, Time.deltaTime), 0, 0);
        }
        if (doneEvents == numberOfEvents && coroutineDone) GetComponent<BoxCollider>().enabled = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(HelpCoroutine());
            if (disablePlayerMovement)
            {
                numberOfEvents++;
                DisablePlayerMovement();
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
            if (lights)
            {
                numberOfEvents++;
                LightsOnOff(lightsOnOff);
            }
            if (jump)
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
            if (throW)
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
            if (openDoor)
            {
                numberOfEvents++;
                OpenDoor();
            }
            if (interactDoor)
            {
                numberOfEvents++;
                InteractDoor();
            }

            gameObject.GetComponent<BoxCollider>().enabled = false;
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
        cc.enabled = true;
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
        bool IsTorchOn = camera.GetComponentInChildren<Light>().enabled;

        if (DisableTorch)
        {
            camera.GetComponentInChildren<Light>().enabled = false;
        }

        otherObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        if (DisableTorch && IsTorchOn)
        {
            camera.GetComponentInChildren<Light>().enabled = true;
        }

        otherObject.gameObject.SetActive(false);
        doneEvents++;
    }
    private IEnumerator AudioThings()
    {
        yield return new WaitForSeconds(2);
    }
    public void DisablePlayerMovement()
    {
        //player.GetComponent<CharacterController>().enabled = false;
        cc.enabled = false;
        StartCoroutine(PlayerMovementCoroutine(delayBeforeMovingAgain));
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
    public void LightsOnOff(bool value)
    {
        foreach (Light light in Lights)
        {
            light.enabled = value;
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
    public void OpenDoor()
    {
        doorController.OpenDoor();
        doorController.doorLocked = true;
    }
    public void InteractDoor()
    {
        doorController.LockedDoor();
    }
    public void Sounds()
    {
        if (play)
        {
            //PlaySound(clipName);
            //audioSource(clipName);
            StartCoroutine(AudioHelper.FadeIn(audioSource, fadeTime));
        }
        if (stop)
        {
            //StopSound(stopClipName);
            StartCoroutine(AudioHelper.FadeOut(audioSource, fadeTime));
        }
        StartCoroutine(AudioThings());
    }
}