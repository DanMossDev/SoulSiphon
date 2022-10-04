using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip[] sfx, float volume = 1)
    {
        audioSource.PlayOneShot(sfx[Random.Range(0, sfx.Length)], volume);
    }
    public void StopSFX()
    {
        audioSource.Stop();
    }
}
