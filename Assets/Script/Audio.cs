using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
  public AudioSource src;
  public AudioClip audio;

    public void playSound()
    {
        src.clip = audio;
        src.Play();
    }
}
