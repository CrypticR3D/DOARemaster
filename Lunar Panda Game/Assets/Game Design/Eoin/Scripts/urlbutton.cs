using UnityEngine;

public class urlbutton : MonoBehaviour
{
    public string Url;

    public void Open()
    {
        Application.OpenURL("https://lunar-panda-studios.itch.io/");
        Debug.Log("is this working?");
    }

}