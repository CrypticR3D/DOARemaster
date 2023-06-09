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
    private GameObject PadlockParent;
    private PadlockInteraction PadlockInteractionScript;
    UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();

        //References the parent object and its script
        PadlockParent = transform.parent.gameObject;
        PadlockInteractionScript = PadlockParent.GetComponent<PadlockInteraction>();
        //Matches the current value and rotation with the current code value in the parent script
        currentNumber = PadlockInteractionScript.getCurrentCode(digitPlacement);
        transform.Rotate(0, 0, (currentNumber * rotationIncrement));
    }

    private void OnMouseOver()
    {
        if (!uIManager.isPaused)
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
                PadlockInteractionScript.changeCurrentCode(digitPlacement, currentNumber);
                PadlockInteractionScript.checkPuzzleComplete();
            }
        }
    }
}
