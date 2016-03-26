using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerBase : NetworkBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    public GameObject Explorer;
    Button SpawnExplorerBtn, PassTurnButton;
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
            SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer());

            PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
            PassTurnButton.onClick.AddListener(() => Cmd_PassTurn());
        }
    }

    [Command]
    public void Cmd_SpawnExplorer()
    {
        GameObject go = (GameObject)Instantiate(Explorer, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

        Rpc_SetObjectOwner(go);
        //NetworkServer.SpawnWithClientAuthority(go, this.gameObject);
        //go.GetComponent<networkComponentVerification>().enabled = true;
    }

    [ClientRpc]
    public void Rpc_SetObjectOwner(GameObject LastCreatedObject)
    {
        LastCreatedObject.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
    }

    [Command]
    public void Cmd_PassTurn()
    {
        gameManager.curTurn++;

        if (gameManager.curTurn > gameManager.MaxTurns)
            gameManager.curTurn = 1;

        //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
        for (int i = 0; i < AllFriendlyUnits.Length; i++)
        {
            AllFriendlyUnits[i].GetComponent<UnitManager>().curActions = AllFriendlyUnits[i].GetComponent<UnitManager>().MaxActions;
        }

        Rpc_PassTurn();
    }

    [ClientRpc]
    public void Rpc_PassTurn()
    { 
        //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
        for (int i = 0; i < AllFriendlyUnits.Length; i++)
        {
            AllFriendlyUnits[i].GetComponent<UnitManager>().curActions = AllFriendlyUnits[i].GetComponent<UnitManager>().MaxActions;
        }
    }

    [Command]
    public void Cmd_MoveUnit(GameObject Obj , Vector3 pos)
    {
        Obj.transform.position = pos;
        Rpc_MoveUnit(Obj, pos);
    }

    [ClientRpc]
    public void Rpc_MoveUnit(GameObject Obj, Vector3 pos)
    {
        Obj.transform.position = pos;
    }
}