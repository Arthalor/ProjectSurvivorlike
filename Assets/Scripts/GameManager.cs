using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEditor;
using System.ComponentModel;

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
    public GamePlayManager gamePlayManager = default;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        gamePlayManager = GameObject.Find("SceneManager").GetComponent<GamePlayManager>();
    }

    public void ReloadScene() 
    {
        ReloadCurrentScene.Reload();
    }
}