using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    public GameMaster gameMaster = null;

    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    public void OnClickButtonStartGame()
    {
        gameMaster.SwitchScene(GameMaster.SceneView.GameScene);
    }
}
