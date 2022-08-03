using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light light;

    private float minWaitTime = 1;
    private float maxWaitTime = 10;

    private float stopMinWaitTime = 1;
    private float stopMaxWaitTime = 10;

    private float minFlicks = 1;
    private float maxFlicks = 10;


    private bool isOn;
    void Start()
    {
        isOn = true;
        light = GetComponent<Light>();
        StartCoroutine(Flashing());
    }
    IEnumerator Flashing()
    {
        while(true)
        {
            if (isOn)
            {
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                light.enabled = !light.enabled;
                isOn = false;
            }
            if (!isOn)
            {
                for(int i = 0; i <= Random.Range(minFlicks, maxFlicks) * 2; i++)
                {
                    yield return new WaitForSeconds(Random.Range(stopMinWaitTime, stopMaxWaitTime));
                    light.enabled = !light.enabled;
                    print(".");
                    isOn = true;
                }
                
            }
        }
        
    }
}
