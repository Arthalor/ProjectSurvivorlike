using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public Transform player = default;
    public PlayerManager playerManager = default;
    public GamePlayManager gamePlayManager = default;

    private bool isPaused = false;

    public bool IsPaused() 
    {
        return isPaused;
    }

    public void TogglePause() 
    {
        if (isPaused) UnPause();
        else Pause();
    }

    public void UnPause() 
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause() 
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ReloadScene() 
    {
        UnPause();
        ReloadCurrentScene.Reload();
    }

    public void LoadMainMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}