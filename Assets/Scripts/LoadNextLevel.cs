using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip[] levelComplete;
    GlobalAudio globalAudio;
    bool isLoading = false;
    
    void Start() {
        globalAudio = FindObjectOfType<GlobalAudio>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !isLoading) StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        isLoading = true;
        globalAudio.PlaySFX(levelComplete);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
