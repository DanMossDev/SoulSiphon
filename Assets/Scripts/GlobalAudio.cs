using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    static AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySFX(AudioClip[] sfx, float volume = 1)
    {
        audioSource.PlayOneShot(sfx[Random.Range(0, sfx.Length)], volume);
    }
    public void StopSFX()
    {
        audioSource.Stop();
    }
}
