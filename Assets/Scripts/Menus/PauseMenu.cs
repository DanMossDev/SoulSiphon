using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public static bool gamePaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;
    [SerializeField] PlayerInput playerInput;

    public void OnPause()
    {
        if (gamePaused) Resume();
        else Pause();
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        playerInput.ActivateInput();
        gamePaused = false;
    }

    void Pause()
    {
        main.SetActive(true);
        options.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        playerInput.DeactivateInput();
        gamePaused = true;
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
