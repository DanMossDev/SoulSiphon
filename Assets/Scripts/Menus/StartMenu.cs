using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleOptions()
    {
        main.SetActive(!main.activeSelf);
        options.SetActive(!options.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
