using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerBase : NetworkBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    public GameObject Explorer;
    public GameObject Warrior;
    public GameObject Archer;
    Button SpawnExplorerBtn, PassTurnButton, SpawnGuerreiroBtn, SpawnArqueiroBtn;
    [SyncVar]
    public int PlayerBaseID;
    public bool Occupied;

    void Start()
    { 

        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();

        if(isLocalPlayer)
            PlayerBaseID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;

        if (PlayerBaseID == playerManager.PlayerID)
        {
            if (PlayerBaseID == 1)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 45, 0);
            }
            if (PlayerBaseID == 2)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 135, 0);
            }
            if (PlayerBaseID == 3)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 330, 0);
            }
            if (PlayerBaseID == 4)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 210, 0);
            }
        }

        if (PlayerBaseID == playerManager.PlayerID)
        {
            SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
            SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer(false));

            SpawnGuerreiroBtn = GameObject.Find("SpawnWarrior").GetComponent<Button>();
            SpawnGuerreiroBtn.onClick.AddListener(() => Cmd_SpawnGuerreiro());

            SpawnArqueiroBtn = GameObject.Find("SpawnArcher").GetComponent<Button>();
            SpawnArqueiroBtn.onClick.AddListener(() => Cmd_SpawnArqueiro());

            PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
            PassTurnButton.onClick.AddListener(() => Cmd_PassTurn());

            //Cmd_SpawnExplorer(true);
        }
    }

    void Update()
    {
        if (gameManager.curTurn == playerManager.MyTurn)
        {
            PassTurnButton.interactable = true;

            if(Occupied == false)
            {
                SpawnExplorerBtn.interactable = true;
                SpawnGuerreiroBtn.interactable = true;
                SpawnArqueiroBtn.interactable = true;
            }
            else
            {
                SpawnExplorerBtn.interactable = false;
                SpawnGuerreiroBtn.interactable = false;
                SpawnArqueiroBtn.interactable = false;
            }             

        }
        else
        {
            PassTurnButton.interactable = false;
            SpawnExplorerBtn.interactable = false;
            SpawnGuerreiroBtn.interactable = false;
            SpawnArqueiroBtn.interactable = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
            Occupied = true;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unit")
            Occupied = false;
    }

    [Command]
    public void Cmd_SpawnExplorer( bool isFirst )
    {
        GameObject go = (GameObject)Instantiate(Explorer, this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
        if (isFirst == true)
            go.GetComponent<UnitManager>().curActions = 3;
        
        Rpc_SetObjectOwner(go);
    }

    [Command]
    public void Cmd_SpawnGuerreiro()
    {
        GameObject go = (GameObject)Instantiate(Warrior, this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, this.gameObject);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

        Rpc_SetObjectOwner(go);
    }


    [Command]
    public void Cmd_SpawnArqueiro()
    {
        GameObject go = (GameObject)Instantiate(Archer, this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, this.gameObject);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

        Rpc_SetObjectOwner(go);
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
    public void Cmd_MoveUnit(GameObject Obj, GameObject Tile)
    {

        Obj.GetComponent<UnitManager>().Rpc_MoveTowardsPoint(new Vector3(Tile.transform.position.x , Tile.transform.position.y + 0.15f , Tile.transform.position.z ));
        

        Rpc_MoveUnit(Obj, Tile);

    }

    [ClientRpc]
    public void Rpc_MoveUnit(GameObject Obj, GameObject Tile)
    {

        Obj.GetComponent<UnitManager>().Rpc_MoveTowardsPoint(new Vector3(Tile.transform.position.x, Tile.transform.position.y + 0.15f, Tile.transform.position.z));
        
    }

    [Command]
    public void Cmd_UpdateSteppingOnTile(GameObject tile , GameObject Obj)
    {
        
        tile.GetComponent<TileManager>().SteppingObject = Obj;
        Rpc_UpdateSteppingOnTile(tile, Obj);
    }

    [ClientRpc]
    public void Rpc_UpdateSteppingOnTile(GameObject tile, GameObject Obj)
    {
        
        tile.GetComponent<TileManager>().SteppingObject = Obj;
    }
}