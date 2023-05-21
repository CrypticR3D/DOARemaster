using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//New Glow script by Eoin//
public class GlowMouseOver : MonoBehaviour
{
    private GameObject m_EmissiveObject;
    private bool isGlowing;
    Color customColor = new Color(0.5f, 0.3f, 0f, 1.0f);
    private float speed = 1.0f;
    //private GameObject Player;
    //private Collider PlayerCollider;

    void Start()
    {
        //Player = GameObject.FindWithTag("Player"); ;
        //PlayerCollider = Player.GetComponentInChildren<SphereCollider>();
        isGlowing = false;
        m_EmissiveObject = gameObject;
    }

    private void OnMouseOver()
    {
        isGlowing = true;
    }

    private void OnMouseExit()
    {
        isGlowing = false;
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