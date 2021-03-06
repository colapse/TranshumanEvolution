﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SerializedMonoBehaviour
{
    public enum SceneView{MainMenuScene, GameScene}

    [SerializeField]
    public Dictionary<SceneView, string> sceneNames;
    public SceneView currentSceneView;

    [SerializeField]
    public GameSetupData gameSetupData;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(SceneView sceneView, bool reload = false)
    {
        if (sceneView == currentSceneView && !reload || sceneNames == null || !sceneNames.ContainsKey(sceneView))
            return;
        
        var loadSceneOP = SceneManager.LoadSceneAsync(sceneNames[sceneView]);
        loadSceneOP.allowSceneActivation = true;
        loadSceneOP.completed += op =>
        {
            if (sceneView == SceneView.GameScene)
                StartCoroutine(InitNewGame());
            
            /*if (sceneNames.ContainsKey(currentSceneView))
            {
                //Debug.Log(sceneNames[currentSceneView]);
                //SceneManager.UnloadSceneAsync(sceneNames[currentSceneView]);
            }*/

            currentSceneView = sceneView;
        };
    }

    private IEnumerator InitNewGame()
    {
        yield return new WaitForEndOfFrame();
        
        var worldManager = FindObjectOfType<WorldManager>();
        if (worldManager == null)
        {
            GameObject worldManagerGo = Instantiate(new GameObject());
            worldManager = worldManagerGo.AddComponent<WorldManager>();
        }
        worldManager.gameSetupData = gameSetupData;
        worldManager.InitWorld();

        yield return 0;
    }
}
