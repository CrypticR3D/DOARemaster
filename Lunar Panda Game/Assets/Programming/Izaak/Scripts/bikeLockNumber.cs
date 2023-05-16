using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bikeLockNumber : MonoBehaviour
{
    public string audioClipName;

    [Header("Digit Values")]
    [Tooltip("Where this number places in the sequence")]
    public int digitPlacement;

    private float rotationIncrement = 36;
    private int currentNumber = 0;
    private GameObject bikeLockParent;
    private PadlockInteraction bikeLockScript;

    // Start is called before the first frame update
    void Start()
    {
        //References the parent object and its script
        bikeLockParent = transform.parent.gameObject;
        bikeLockScript = bikeLockParent.GetComponent<PadlockInteraction>();
        //Matches the current value and rotation with the current code value in the parent script
        currentNumber = bikeLockScript.getCurrentCode(digitPlacement);
        transform.Rotate(0, 0, (currentNumber * rotationIncrement));
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentNumber <= 0)
            {
                currentNumber = 9;
            }
            else
            {
                currentNumber--;
            }
            transform.Rotate(0, 0, -rotationIncrement);
            SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
            bikeLockScript.changeCurrentCode(digitPlacement, currentNumber);
            bikeLockScript.checkPuzzleComplete();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (currentNumber >= 9)
            {
                currentNumber = 0;
            }
            else
            {
                currentNumber++;
            }
            transform.Rotate(0, 0, rotationIncrement);
            SoundEffectManager.GlobalSFXManager.PlaySFX(audioClipName);
            bikeLockScript.changeCurrentCode(digitPlacement, currentNumber);
            bikeLockScript.checkPuzzleComplete();
        }
    }
}
