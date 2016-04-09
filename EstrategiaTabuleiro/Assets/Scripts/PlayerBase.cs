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

        PlayerBaseID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;

        if (PlayerBaseID == playerManager.PlayerID)
        {
            SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
            SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer());

            SpawnGuerreiroBtn = GameObject.Find("SpawnWarrior").GetComponent<Button>();
            SpawnGuerreiroBtn.onClick.AddListener(() => Cmd_SpawnGuerreiro());

            SpawnArqueiroBtn = GameObject.Find("SpawnArcher").GetComponent<Button>();
            SpawnArqueiroBtn.onClick.AddListener(() => Cmd_SpawnArqueiro());

            PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
            PassTurnButton.onClick.AddListener(() => Cmd_PassTurn());
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
    public void Cmd_SpawnExplorer()
    {
        GameObject go = (GameObject)Instantiate(Explorer, this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        //NetworkServer.Spawn(go);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
        

        Rpc_SetObjectOwner(go);
    }

    [Command]
    public void Cmd_SpawnGuerreiro()
    {
        GameObject go = (GameObject)Instantiate(Warrior, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

        Rpc_SetObjectOwner(go);
    }

    [Command]
    public void Cmd_SpawnArqueiro()
    {
        GameObject go = (GameObject)Instantiate(Archer, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
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
        //Obj.transform.LookAt(Tile.transform.position);
        //Obj.transform.position = Vector3.MoveTowards(Obj.transform.position , Tile.transform.position, Time.deltaTime * 2);

        Obj.GetComponent<UnitManager>().MoveTowardsPoint(Tile.transform.position);

        Rpc_MoveUnit(Obj, Tile);

        
    }

    [ClientRpc]
    public void Rpc_MoveUnit(GameObject Obj, GameObject Tile)
    {
        //Obj.transform.position = pos;

        //Obj.transform.LookAt(Tile.transform.position);
        //Obj.transform.position = Vector3.MoveTowards(Obj.transform.position, Tile.transform.position, Time.deltaTime * 2);

        Obj.GetComponent<UnitManager>().MoveTowardsPoint(Tile.transform.position);

       // Obj.transform.LookAt(Tile.transform.position);
        //Obj.transform.position = Vector3.MoveTowards(this.transform.position, Tile.transform.position, Time.deltaTime);
        
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

   // [Command]
   // public void Cmd_UpdateUnitPosition(GameObject Unit , Vector3 Pos)
  //  {

       // Unit.transform.position = Pos;
       // Unit.transform.LookAt(Pos);
  //  }
}