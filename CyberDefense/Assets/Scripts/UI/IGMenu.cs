using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IGMenu : MonoBehaviour {

    [Header("Camera's")]
    public GameObject mainCamera;
    public GameObject pauseCamera;

    [Header("Menu's")]
    public GameObject inGameUI;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("")]
    public static IGMenu instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            inGameUI.SetActive(false);
            pauseCamera.SetActive(true);
            mainCamera.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnContinueClick()
    {
        Time.timeScale = 1;
        inGameUI.SetActive(true);
        pauseCamera.SetActive(false);
        mainCamera.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void OnBackToMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainLevel");
    }
}