using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//New Glow script by Eoin//
public class GlowWhenLookedAt : MonoBehaviour
{
    public GameObject m_EmissiveObject;
    public bool isGlowing;
    Color customColor = new Color(0.5f, 0.3f, 0f, 1.0f);
    public float speed = 1.0f;
    public GameObject Player;
    public Collider PlayerCollider;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerCollider = Player.GetComponentInChildren<SphereCollider>();
        isGlowing = false;
        m_EmissiveObject = gameObject;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GlowTag")
        {
            if (m_EmissiveObject.tag == "DirectGlow")
            {
                //isGlowing = false;
            }
            else
            {
                isGlowing = true;
            }
            
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "GlowTag")
        {
            isGlowing = false;
        }
        
    }
    void Update()
    {
        if (isGlowing == true)
        {
            float frac = Mathf.PingPong(Time.time, 1) * speed;
            m_EmissiveObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.Lerp(Color.black, customColor, frac));
        }
        else
        {
            m_EmissiveObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.black);
        }
    }

    public void ToggleGlowingMat()
    {
        isGlowing = true;
    }
}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

////New Glow script by Eoin//
//public class GlowWhenLookedAt : MonoBehaviour
//{
//    [HideInInspector] public bool isGlowing;
//    [HideInInspector] public Material baseMaterial; //need this for the input script thingie but dont want in inspector
//    [SerializeField] public Material glowingMaterial;
//    [Header("Children")]
//    [Tooltip("Only use this if the object you want to make glow has children that also need to glow")]
//    [SerializeField] List<MeshRenderer> childrenThatNeedGlow;
//    [SerializeField] List<Material> childrenFresnelMat;
//    List<Material> childrenBaseMat = new List<Material>();
//    public GameObject m_EmissiveObject;
//    public bool isGlowing;
//    Color customColor = new Color(0.5f, 0.3f, 0f, 1.0f);
//    public float speed = 1.0f;
//    public GameObject Player;
//    public Collider PlayerCollider;

//    [SerializeField] ToolTipType tooltip;
//    [SerializeField] Text tooltipTxt;
//    [SerializeField] CanvasGroup tooltipGroup;
//    PlayerPickup pickup;
//    [SerializeField] float waitTime = 0.5f;

//    bool isFading = false;

//    void Awake()
//    void Start()
//    {
//        baseMaterial = gameObject.GetComponent<MeshRenderer>().material;
//        pickup = FindObjectOfType<PlayerPickup>();
//        for (int i = 0; i < childrenThatNeedGlow.Count; i++)
//        {
//            childrenBaseMat.Add(childrenThatNeedGlow[i].material);
//        }
//        Player = GameObject.FindWithTag("Player");
//        PlayerCollider = Player.GetComponentInChildren<SphereCollider>();
//        isGlowing = false;
//        m_EmissiveObject = gameObject;
//    }

//    //this just toggles the material between the glowing one and the base one
//    public void ToggleGlowingMat()
//    void OnTriggerEnter(Collider other)
//    {
//        isGlowing = !isGlowing;
//        if (isGlowing && !isFading && tooltip != null && pickup.heldItem == null)
//            if (other.tag == "GlowTag")
//            {
//                isFading = true;
//                StartCoroutine(FadeTooltips());
//            }
//        //if the glowing bool is true, set the material to be glowing, if its false, set it to the base material
//        this.gameObject.GetComponent<MeshRenderer>().material = isGlowing ? glowingMaterial : baseMaterial;
//        if (childrenThatNeedGlow.Count > 0)
//        {
//            for (int i = 0; i < childrenThatNeedGlow.Count; i++)
//            {
//                childrenThatNeedGlow[i].materials[0] = isGlowing ? childrenFresnelMat[i] : childrenBaseMat[i];
//            }
//            isGlowing = true;
//        }

//    }

//    IEnumerator FadeTooltips()
//    void OnTriggerExit(Collider other)
//    {
//        tooltipTxt.text = tooltip.text;
//        for (float t = 0f; t < tooltip.fadeTime; t += Time.deltaTime)
//            if (other.tag == "GlowTag")
//            {
//                float normalizedTime = t / tooltip.fadeTime;
//                tooltipGroup.alpha = Mathf.Lerp(0, 1, normalizedTime);
//                yield return null;
//                isGlowing = false;
//            }
//        yield return new WaitForSeconds(waitTime);
//        while (InteractRaycasting.Instance.raycastInteract(out RaycastHit hit))


//    }
//    void Update()
//    {
//        if (isGlowing == true)
//        {
//            if (hit.transform.gameObject == gameObject && pickup.heldItem == null)
//            {
//                yield return new WaitForEndOfFrame();
//                yield return null;
//            }
//            else
//            {
//                break;
//            }
//            float frac = Mathf.PingPong(Time.time, 1) * speed;
//            m_EmissiveObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.Lerp(Color.black, customColor, frac));
//        }
//        isFading = false;
//        for (float t = 0f; t < tooltip.fadeTime; t += Time.deltaTime)
//        else
//                {
//                    float normalizedTime = t / tooltip.fadeTime;
//                    tooltipGroup.alpha = Mathf.Lerp(1, 0, normalizedTime);
//                    yield return null;
//                    m_EmissiveObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.black);
//                }
//        tooltipGroup.alpha = 0;
//        yield return null;
//    }
//}
//}