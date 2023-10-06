using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject deathUI = default;
    [SerializeField] private GameObject winUI = default;
    [SerializeField] private GameObject sharedUI = default;
    [SerializeField] private GameObject pauseUI = default;
    [SerializeField] private GameObject statUI = default;
    [SerializeField] private GameObject levelUpUI = default;
    [SerializeField] private InGameUI inGameUI = default;
    [Space]
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
        playerControls.enabled = !GameManager.Instance.IsPaused();

        if (endlessMode) return;

        winTimer.Tick(Time.deltaTime);

        if (winTimer.Finished()) GameWon();
    }

    public void LevelUpEvent() 
    {
        pauseBlock = true;
        GameManager.Instance.Pause();
        levelUpUI.SetActive(true);
        inGameUI.UpdateLevelUpUI();
    }

    public void LevelUpCompleted()
    {
        levelUpUI.SetActive(false);
        GameManager.Instance.UnPause();
        pauseBlock = false;
    }

    public void GameWon()
    {
        pauseBlock = true;
        winUI.SetActive(true);
        sharedUI.SetActive(true);
        GameManager.Instance.Pause();
        ShowStatUI(true);
        UpdatePostGameUI();
    }

    public void ContinueEndless() 
    {
        pauseBlock = false;
        endlessMode = true;
        GameManager.Instance.UnPause();
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
        UpdatePostGameUI();
    }

    public void PauseKeyPressed() 
    {
        if (pauseBlock) return;

        bool pauseState = GameManager.Instance.IsPaused();
        ShowStatUI(!pauseState);
        pauseUI.SetActive(!pauseState);
        GameManager.Instance.TogglePause();
    }

    private void ShowStatUI(bool show) 
    {
        inGameUI.UpdateStatUI();
        statUI.SetActive(show);
    }

    public void UpdatePostGameUI()
    {
        inGameUI.UpdatePostGameUI();
    }
}