using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameSetupData gameSetupData;
    public Player player;

    public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void InitWorld()
    {
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            GameObject playerGo = Instantiate(new GameObject());
            player = playerGo.AddComponent<Player>();
        }
        player.gameSetupData = gameSetupData;
        player.InitData();

        Canvas canvas = FindObjectOfType<Canvas>();
        Vector3 newCamPos = Camera.main.transform.position;
        newCamPos.x = player.transhuman.transform.position.x; /*- canvas.pixelRect.width/2 + 200;*/
        //Camera.main.transform.position = newCamPos;
        var screenPoint = new Vector3(100,canvas.pixelRect.height, Camera.main.nearClipPlane);
        newCamPos.x -= newCamPos.x-Camera.main.ScreenToWorldPoint(screenPoint).x;
        
        Camera.main.transform.position = newCamPos;
        
        uiManager?.InitUI();
        
        player?.AdvanceToNextTimePeriod();
    }
}
