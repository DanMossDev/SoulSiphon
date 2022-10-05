using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMusic : MonoBehaviour
{
    [Space]
    [Header("Music for each level")]
    [SerializeField] AudioClip mainMenu;
    [SerializeField] AudioClip level1;
    [SerializeField] AudioClip level2;
    [SerializeField] AudioClip level3;

    AudioSource audioSource;

    void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip currentTrack;

        switch (scene.buildIndex)
        {
            case 0:
            case 1:
                currentTrack = mainMenu;
                break;
            case 2:
                currentTrack = level1;
                break;
            default: 
                currentTrack = level1;
                break;
        }
        if (audioSource.clip != currentTrack)
        {
            audioSource.clip = currentTrack;
            audioSource.Play();
        }
    }
}
