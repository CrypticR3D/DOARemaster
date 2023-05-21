using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


//SINGLETON
//Manager of Managers
//Scene Loaders
//Tracking Player:
//Current Level
//Others variables that could be useful

public enum GameState //Only Basic states
{
    MENU, GAME, PAUSE, QUIT
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameStates;
    public int whichLevel = 0;
    public Room currentRoom;
    public GameObject player;
    internal Vector3 location;
    internal Quaternion rotation;
    public Inventory inventory;
    public PuzzleData completion;
    internal bool canSave;
    internal int saveFile = 1;
    internal string currentScene;
    internal bool subtitles;
    internal List<DocumentData> docInventory;




    private void Awake()
    {



        docInventory = new List<DocumentData>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        completion = FindObjectOfType<PuzzleData>();
        player = FindObjectOfType<CharacterController>().gameObject;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void save(bool mainSave)
    {
        currentScene = SceneManager.GetActiveScene().name;
        Database.current.locationUpdate();
        player = FindObjectOfType<CharacterController>().gameObject;
        inventory = FindObjectOfType<Inventory>();
        completion = FindObjectOfType<PuzzleData>();
        location = player.transform.position;
        rotation = player.transform.rotation;
        if(mainSave)
        {
            SaveSystem.asignPath(saveFile);
        }
        else
        {
            SaveSystem.asignPath(0);
        }
        SaveSystem.asignPath(saveFile);
        saveAsync();
    }

    public async void saveAsync()
    {
        var result = await Task.Run(() =>
        {
            SaveSystem.save(saveFile);

            return true;
        }
        );

        UIManager.Instance.autoSavingPromptHide();
    }

    IEnumerator loadScene(GameData data)
    {
        bool sceneJustLoad = false;

        if(whichLevel != data.whichLevel)
        {
            whichLevel = data.whichLevel;
            StartCoroutine(FindObjectOfType<LevelManager>().FadeLoadingScreen(data.sceneName));

            yield return new WaitForEndOfFrame();

            sceneJustLoad = true;

            player = FindObjectOfType<playerMovement>().gameObject;
            inventory = FindObjectOfType<Inventory>();
            completion = FindObjectOfType<PuzzleData>();
        }

        player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        player.transform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
        player.transform.rotation = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2],0);

        //inventory.itemInventory = data.itemInven;
        int index = 0;

       index = 0;

        inventory.documentInventory.Clear();

        foreach (string docId in data.docInven)
        {
            if (docId != null)
            {
                inventory.documentInventory.Add(Database.current.allDocs[int.Parse(docId)]);

                GameObject document = GameObject.Find(inventory.documentInventory[index].prefab.name);

                if (document != null)
                {
                    document.SetActive(false);
                }
            }

            index += 1;
        }

        index = 0;
        inventory.storyNotesInventory.Clear();

        //Story documents inventory 
        foreach (string storyID in data.docInven)
        {
            if (storyID != null)
            {
                inventory.storyNotesInventory.Add(Database.current.allStoryNotes[int.Parse(storyID)]);
                GameEvents.current.onTriggerStoryNotes(Database.current.allStoryNotes[int.Parse(storyID)].id);
            }

            index += 1;
        }

        print("Load");
    }

    public void load(int slot = 4)
    {
        SaveSystem.asignPath(saveFile);
        GameData data = SaveSystem.load();

        if(data != null)
        {
            StartCoroutine(loadScene(data));
        }
        else
        {
            print("No data");
        }
    }

    public void deleteButton(int slot)
    {
        SaveSystem.delete(slot);
    }



    public void currentLevel(int currLevel)
    {
        //Just returning the value of which game scene the player is in
        whichLevel = currLevel;
        switch (whichLevel)
        {
            case 0:
                {
                    currentRoom = Room.NONE;
                    break;
                }
            case 1:
                {
                    currentRoom = Room.CRASHEDTRAIN;
                    break;
                }
            case 2:
                {
                    currentRoom = Room.TRAIN;
                    break;
                }
            case 3:
                {
                    currentRoom = Room.HOSPITAL;
                    break;
                }
            case 4:
                {
                    currentRoom = Room.HOTEL;
                    break;
                }
            case 5:
                {
                    currentRoom = Room.CABIN;
                    break;
                }
            default:
                {
                    currentRoom = Room.NONE;
                    whichLevel = 0;
                    break;
                }
        }
        return;
    }
}
