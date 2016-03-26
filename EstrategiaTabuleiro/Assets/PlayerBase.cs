using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerBase : NetworkBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    public GameObject Explorer;
    Button SpawnExplorerBtn;
    [SyncVar]
    public int PlayerBaseID;

    void Start()
    {
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();

        PlayerBaseID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;

        if (PlayerBaseID == playerManager.PlayerID)
        {
            SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
            SpawnExplorerBtn.onClick.AddListener(() => SpawnUnit());
        }
    }

	public void SpawnUnit()
    {
        gameManager.Cmd_SpawnObject(Explorer, this.transform.position);
    }
}