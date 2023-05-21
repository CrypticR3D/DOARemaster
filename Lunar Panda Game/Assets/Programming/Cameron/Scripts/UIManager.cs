using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    [Header("UI Elements")]
    
    [Tooltip("Put the parent empty object of the document system here")]
    [SerializeField] GameObject docViewingSystem;
    [SerializeField] GameObject inventorySystem;
    [SerializeField] CanvasGroup fadeGroup;
    [SerializeField] float fadeDuration;
    public Text storyNotes;
    public Text notesText;
    Inventory inventory;
    public GameObject documentLandscape;
    public GameObject documentPortrait;
    typeWriterTest twt;
    [SerializeField] string audioClipName;
    bool isMoving;

    [Header("Inventory UI")]
    public List<Image> inventoryImages;
    public Text inventoryDescription;
    public Text inventoryName;
    public Image inventorySelect;
    public GameObject descriptionSection;
    public Image bottomRightItem;
    public Image bottomRightPanel;
    bool itemShowing = false;

    [Header("Tooltip UI")]
    public GameObject TooltipSection;
    public Text tooltipText;
    public Image tooltipImage;

    InventoryMenuToggle inventoryMenuToggle;
    PauseButtonToggle pauseButtonToggle;

    [Header("InGame UI")]
    [SerializeField] GameObject TorchUI;
    [SerializeField] GameObject InvPanUI;
    [SerializeField] GameObject crosshair;

    [Header("Puzzle UI")]
    [SerializeField] GameObject PuzzleTooltipUI;

    public bool isOnInventory;
    public bool isOnPuzzle;
    public bool uiHidden;
    public bool isPaused;

    void Awake()
    {
        uiHidden = true;
        //setting up singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        if (!isOnPuzzle)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }

        inventory = FindObjectOfType<Inventory>();
        inventoryMenuToggle = FindObjectOfType<InventoryMenuToggle>();
        pauseButtonToggle = FindObjectOfType<PauseButtonToggle>();
    }

    private void Update()
    {
        if (isOnPuzzle || isOnInventory || isPaused)
        {
            if (!uiHidden)
            {
                ShowTooltip();
                HideUI();
                Debug.Log("HidingUI");
            }
        }
        else
        {
            if (uiHidden)
            {
                HideTooltip();
                ShowUI();
                Debug.Log("ShowingUI");
            }
        }
    }

    public void ShowUI()
    {
        TorchUI.SetActive(true);
        InvPanUI.SetActive(true);
        crosshair.SetActive(true);

        uiHidden = false;
    }

    public void HideUI()
    {
        TorchUI.SetActive(false);
        InvPanUI.SetActive(false);
        crosshair.SetActive(false);

        uiHidden = true;
    }

    public void ShowTooltip()
    {
        PuzzleTooltipUI.SetActive(true);
    }

    public void HideTooltip()
    {
        PuzzleTooltipUI.SetActive(false);
    }

    public void autoSavingPromptShow()
    {
        //autoSavingSection.SetActive(true);
    }
    public void autoSavingPromptHide()
    {
        //autoSavingSection.SetActive(false);
    }
    public void toolTipInteract(ToolTipType type)
    {
        TooltipSection.SetActive(true);
        tooltipText.text = type.text;
        tooltipImage.sprite = type.KeyboardSprite;
    }
    public void toolTipHide()
    {
        TooltipSection.SetActive(false);
    }
    public void itemEquip(ItemData data)
    {
        Color colour = bottomRightItem.color;

        if (data != null)
        {
            if (!isMoving)
            {
                bottomRightItem.color = new Color(colour.r, colour.g, colour.b, 1);
            }
            bottomRightItem.sprite = data.itemImage;
        }
        else
        {
            bottomRightItem.color = new Color(colour.r, colour.g, colour.b, 0);
            bottomRightItem.sprite = null;
        }

    }
    public void itemFade(bool isMove)
    {
        isMoving = isMove;

        Color colour = bottomRightPanel.color;
        Color colourItem = bottomRightItem.color;

        if (isMoving)
        {
            bottomRightPanel.color = new Color(colour.r, colour.g, colour.b, 0.10f);
            //StartCoroutine(fadeBottomRightPanel(0.10f));
            if (itemShowing)
            {
                bottomRightItem.color = new Color(colourItem.r, colourItem.g, colourItem.b, 0.10f);
                //StartCoroutine(fadeBottomRightItem(0.10f));
            }

        }
        else
        {
            bottomRightPanel.color = new Color(colour.r, colour.g, colour.b, 1);
            //StartCoroutine(fadeBottomRightPanel(1));
            if (itemShowing)
            {
                //StartCoroutine(fadeBottomRightItem(1));
                bottomRightItem.color = new Color(colourItem.r, colourItem.g, colourItem.b, 1);
            }
        }
    }
    public IEnumerator FadePanelIn()
    {

        float elapsedTime = 0;
        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }
    }
    public void showDocument(DocumentData data, ViewDocument documentScript)
    {
        if (data.isLandscape)
        {
            documentLandscape.GetComponent<Image>().sprite = data.documentImage;
            documentLandscape.SetActive(true);
        }
        else
        {
            documentPortrait.GetComponent<Image>().sprite = data.documentImage;
            documentPortrait.SetActive(true);
        }

        if (documentScript != null)
        {
            if (!documentScript.inInventory)
            {
                inventory.addItem(data);
            }
            documentScript.showDoc = true;
        }
    }
    public void hideDocument(DocumentData data, ViewDocument documentScript)
    {
        if (documentScript != null)
        {
            documentScript.showDoc = false;
        }

        if (data.isLandscape)
        {
            documentLandscape.SetActive(false);
        }
        else
        {
            documentPortrait.SetActive(false);
        }

    }
    public void showingText(DocumentData data, ViewDocument documentScript)
    {
        notesText.text = data.docText;
        if (documentScript != null)
        {
            documentScript.showText = true;
        }
        notesText.transform.parent.gameObject.SetActive(true);
        //Show text when pressed
    }
    public void hideText(ViewDocument documentScript)
    {
        if (documentScript != null)
        {
            documentScript.showText = false;
        }
        notesText.transform.parent.gameObject.SetActive(false);
        //Hide text when pressed
    }
    public void inventoryItemAdd(ItemData data, int slot)
    {
        inventoryImages[slot].sprite = data.itemImage;
        inventoryImages[slot].color = new Color(inventoryImages[slot].color.r, inventoryImages[slot].color.g, inventoryImages[slot].color.b, 1);
    }
    public void inventoryItemSelected(ItemData data, int slot)
    {
        Color colour = inventoryImages[slot].color;
        Color colourSelect = inventorySelect.color;
        Color colourRightImage = bottomRightItem.color;

        if (data != null)
        {
            descriptionSection.SetActive(true);
            itemShowing = true;
            inventoryImages[slot].color = new Color(colour.r, colour.g, colour.b, 1);
            inventorySelect.color = new Color(colourSelect.r, colourSelect.g, colourSelect.b, 1);
            if (!isMoving)
            {
                bottomRightItem.color = new Color(colourRightImage.r, colourRightImage.g, colourRightImage.b, 1);
            }
            inventorySelect.sprite = data.itemImage;
            inventoryImages[slot].sprite = data.itemImage;
            inventoryName.text = data.itemName;
            inventoryDescription.text = data.description;
            data.timesChecked++;
        }
        else
        {
            descriptionSection.SetActive(false);
            itemShowing = false;
            inventoryImages[slot].color = new Color(colour.r, colour.g, colour.b, 0);
            inventorySelect.color = new Color(colourSelect.r, colourSelect.g, colourSelect.b, 0);
            bottomRightItem.color = new Color(colourRightImage.r, colourRightImage.g, colourRightImage.b, 0);
            inventorySelect.sprite = null;
            inventoryImages[slot].sprite = null;
            bottomRightItem.sprite = null;
            inventoryName.text = "";
            inventoryDescription.text = "";
        }
    }
    public void removeItemImage(int slot)
    {
        Color colour = inventoryImages[slot].color;
        Color colourSelect = inventorySelect.color;

        inventoryImages[slot].sprite = null;
        inventoryImages[slot].color = new Color(colour.r, colour.g, colour.b, 0);

        if(inventory.selectedItem == slot)
        {
            inventorySelect.color = new Color(colourSelect.r, colourSelect.g, colourSelect.b, 0);
            descriptionSection.SetActive(false);
            itemShowing = false;
            inventorySelect.sprite = null;
            inventoryName.text = "";
            inventoryDescription.text = "";
        }
    }
    public void updateObject()
    {
        
    }
    public void textToScreen(string dialogue)
    {
        twt.setupText();
        twt.dialogue = dialogue;
        twt.dialogueText.enabled = true;
        twt.BG.enabled = true;
        twt.playText = true;
    }
    public void diableSubtitles()
    {
        twt.dialogueText.enabled = false;
        twt.BG.enabled = false;
    }
    public void toggleMenuVariables()
    {
        //feedbackToggle.canOpen = !feedbackToggle.canOpen;
        inventoryMenuToggle.canOpen = !inventoryMenuToggle.canOpen;
        //journalMenuToggle.canOpen = !journalMenuToggle.canOpen;
        pauseButtonToggle.canOpen = !pauseButtonToggle.canOpen;
    }
   
}
