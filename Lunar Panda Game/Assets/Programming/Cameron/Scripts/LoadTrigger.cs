using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrigger : MonoBehaviour
{
    //[SerializeField] GameObject openedDoor;
    //[SerializeField] GameObject closedDoor;
    [SerializeField] string sceneName;
    //bool loadingScene = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("TipCollider"))
        {
            SceneManager.LoadScene(sceneName);

            ////openedDoor.SetActive(true);
            //if (!loadingScene)
            //{
            //    StartCoroutine(LoadScene());
            //}
        }
    }

    //IEnumerator LoadScene()
    //{
    //    StartCoroutine(FindObjectOfType<LevelManager>().FadeLoadingScreen(sceneName));
    //    //closedDoor.SetActive(false);
    //    yield return null;
    //}
}