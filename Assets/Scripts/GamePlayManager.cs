using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject deathUI = default;
    [SerializeField] private GameObject winUI = default;
    [SerializeField] private GameObject sharedUI = default;
    [SerializeField] private Timer winTimer = default;

    private void Update()
    {
        winTimer.TickTimer(Time.deltaTime);

        if (winTimer.TimerFinished()) GameWon();
    }

    public void GameWon()
    {
        winUI.SetActive(true);
        sharedUI.SetActive(true);
    }

    public void GameLost()
    {
        deathUI.SetActive(true);
        sharedUI.SetActive(true);
    }
}