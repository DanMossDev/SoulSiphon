using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    AudioController audioController;
    void Start()
    {
        audioController = GetComponent<AudioController>();
    }

    public void PlaySFX(AudioClip sfx, float volume = 1 )
    {
        audioController.PlaySFX(sfx, volume);
    }

    public void StopSFX()
    {
        audioController.StopSFX();
    }
}
