using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public void GameOver() 
    {
        Helper.ReloadCurrentScene.Reload();
    }
}