using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    private Light lightSource;
    internal bool onOff = false;

    public Image UIon;
    public Image UIoff;

    public string clipName;

    public float minIntensity;
    public float maxIntensity;

    public float wallIntensityChangeSpeed = 2.0f;

    public float wallDistance;

    public float intensity;

    void Start()
    {
        lightSource = this.gameObject.GetComponent<Light>();
        lightSource.type = LightType.Spot;
    }

    void Update()
    {
        toggleLight();

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, wallDistance))
        {
            float distanceToWall = hit.distance;
            float lerpFactor = Mathf.Clamp01(distanceToWall / wallDistance);
            intensity = Mathf.Lerp(minIntensity, maxIntensity, lerpFactor);
        }
        else
        {
            intensity = maxIntensity;
        }

        lightSource.intensity = intensity;
    }

    void toggleLight()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            onOff = !onOff;

            if (onOff)
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);
                lightSource.enabled = true;
                //UIon.enabled = true;
                //UIoff.enabled = false;
            }
            else
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);
                lightSource.enabled = false;
                //UIon.enabled = false;
                //UIoff.enabled = true;
            }
        }
    }
}
