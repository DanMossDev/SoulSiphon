using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip[] sfx, float volume = 1, bool loop = false)
    {
        if (loop) {
            audioSource.loop = true;
            audioSource.clip = sfx[Random.Range(0, sfx.Length)];
            audioSource.Play();
        }
        else audioSource.PlayOneShot(sfx[Random.Range(0, sfx.Length)], volume);
    }
    public void StopSFX()
    {
        audioSource.Stop();
    }
}
