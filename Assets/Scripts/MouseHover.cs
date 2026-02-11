using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MouseHover : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip mouseHover;


   
    private void MouseOver()
    {
        audioSource.PlayOneShot(mouseHover);
    }
}
