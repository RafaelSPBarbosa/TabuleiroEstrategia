using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchMaking : MonoBehaviour {

    public string www_data = "";
    MatchMakingData data;
    public GameObject CanvasParent;
    public GameObject EntryPrefab;
    List<GameObject> DataDisplay;

    NetManager ClientManager;

	// Use this for initialization
	void Start () {
        Debug.Log("Searching for Matches");
        ClientManager = GameObject.Find("NetManager").GetComponent<NetManager>();
        StartCoroutine("GetServers");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator GetServers()
    {
        string url = "http://www.jaytechmedia.com/autem/list.php";
        WWW www = new WWW(url);
        yield return www;
        www_data = www.text;
        //Debug.Log(www.text);
        //Convert JSON into unity useable array
        data = MatchMakingData.CreateFromJSON(www.text);
        foreach (ServerData server in data.servers)
        {
            GameObject temp = Instantiate(EntryPrefab);
            temp.transform.parent = CanvasParent.transform;
            temp.GetComponent<ServerListEntry>().SetData(server);
            temp.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { ClientManager.MatchMakingConnect(server.ip); });
           // DataDisplay.Add(temp);
        }
        yield return null;
    }
}
