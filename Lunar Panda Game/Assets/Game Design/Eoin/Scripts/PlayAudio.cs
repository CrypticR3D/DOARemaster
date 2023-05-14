using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource aud;
    public void play_clip()
    {
        aud.Play();
    }
}