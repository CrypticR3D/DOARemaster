using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public string LevelName;

    private void OnEnable()
    {
        SceneManager.LoadSceneAsync(LevelName, LoadSceneMode.Single);
    }
}
