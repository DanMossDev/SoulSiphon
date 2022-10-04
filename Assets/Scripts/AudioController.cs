using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip sfx, float volume = 1)
    {
        audioSource.PlayOneShot(sfx, volume);
    }
    public void StopSFX()
    {
        audioSource.Stop();
    }
}
