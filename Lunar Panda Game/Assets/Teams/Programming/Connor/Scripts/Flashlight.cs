using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script by Connor - if designers need any assistance feel free to dm or @ me on Discord

public class Flashlight : MonoBehaviour
{
    private Light lightSource; //creating lightsource to assign at runtime

    //const float maxBatteryLife = 60; //total battery life 
    //float batteryLife = maxBatteryLife; //current battery life will change this later after designers say what they want

    internal bool onOff = false; //Flashlight starts disabled this manages battery consumption 


    //Matej changes - just flickering :)
    //public float minWaitTime = 5;
    //public float maxWaitTime = 8;

    //public float stopMinWaitTime = 0;
    //public float stopMaxWaitTime = 0.2f;

    //public int minFlicks = 2;
    //public int maxFlicks = 3;

    public Image UIon;
    public Image UIoff;

    private bool isOn;
    public string clipName;//Matej changes

    public float intensity = 0f;

    public float minIntensity;
    public float maxIntensity;


    void Start()
    {
        lightSource = this.gameObject.GetComponent<Light>(); //assigning the spotlight to access it
        isOn = true;

        lightSource.type = LightType.Spot;
        //StartCoroutine(Flashing());


    }

    void Update()
    {
        toggleLight(); //runs the toggleLight function.

        lightSource.intensity = intensity;

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, 2))
        {
            Mathf.MoveTowards(intensity, minIntensity, Time.deltaTime / maxIntensity) ;
            //intensity += Time.deltaTime * 60;

        }

        else
        {
            Mathf.MoveTowards(intensity, maxIntensity, Time.deltaTime / minIntensity);
            //intensity -= Time.deltaTime * 60;

        }

        //intensity = Mathf.Clamp(intensity, minIntensity, maxIntensity);

    }

    void toggleLight()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            onOff = !onOff;

            if (onOff)
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);//Matej changes
                lightSource.enabled = true;
                UIon.enabled = true;
                UIoff.enabled = false;
            }
            else
            {
                SoundEffectManager.GlobalSFXManager.PlaySFX(clipName);//Matej changes
                lightSource.enabled = false;
                UIon.enabled = false;
                UIoff.enabled = true;
            }
        }
    }

    //IEnumerator Flashing()
    //{
    //    while (true)
    //    {
    //        if(onOff)
    //        {
    //            if (isOn)
    //            {
    //                lightSource.intensity = 28000;
    //                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
    //                isOn = false;
    //            }
    //            if (!isOn)
    //            {
    //                for (int i = 0; i <= Random.Range(minFlicks, maxFlicks) * 2; i++)
    //                {
    //                    yield return new WaitForSeconds(Random.Range(stopMinWaitTime, stopMaxWaitTime));
    //                    lightSource.intensity = Random.Range(5000, 10000);
    //                    isOn = true;
    //                }
    //            }
    //        } 
    //    }
    //}
}
