using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.Rendering.Universal;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject deathUI = default;
    [SerializeField] private GameObject winUI = default;
    [SerializeField] private GameObject sharedUI = default;
    [SerializeField] private GameObject pauseUI = default;
    [SerializeField] private GameObject statUI = default;
    [SerializeField] private InGameUI inGameUI = default;
    [Space]
    [SerializeField] private Timer winTimer = default;
    [Space]
    [SerializeField] private PlayerControls playerControls = default;

    private bool endlessMode = false;
    private bool pauseBlock = false;

    private void Start()
    {
        GameManager.Instance.UnPause();
        pauseBlock = false;
    }

    private void Update()
    {
        if (endlessMode) return;

        winTimer.TickTimer(Time.deltaTime);

        if (winTimer.TimerFinished()) GameWon();
    }

    public void GameWon()
    {
        pauseBlock = true;
        winUI.SetActive(true);
        sharedUI.SetActive(true);
        GameManager.Instance.Pause();
        playerControls.enabled = false;
        ShowStatUI(true);
    }

    public void ContinueEndless() 
    {
        pauseBlock = false;
        endlessMode = true;
        GameManager.Instance.UnPause();
        playerControls.enabled = true;
        winUI.SetActive(false);
        sharedUI.SetActive(false);
        ShowStatUI(false);
    }

    public void GameLost()
    {
        pauseBlock = true;
        deathUI.SetActive(true);
        sharedUI.SetActive(true);
        ShowStatUI(true);
    }

    public void PauseKeyPressed() 
    {
        if (pauseBlock) return;

        bool pauseState = GameManager.Instance.IsPaused();
        ShowStatUI(!pauseState);
        pauseUI.SetActive(!pauseState);
        playerControls.enabled = pauseState;
        GameManager.Instance.TogglePause();
    }

    private void ShowStatUI(bool show) 
    {
        inGameUI.UpdateStatUI();
        statUI.SetActive(show);
    }
}