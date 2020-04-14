using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameSetupData gameSetupData;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
