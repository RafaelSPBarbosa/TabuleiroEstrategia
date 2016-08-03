using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MatchMaking : MonoBehaviour {

    public string www_data = "";
    MatchMakingData data;
    public GameObject CanvasParent;
    public GameObject EntryPrefab;
    List<GameObject> DataDisplay;

    NetManager ClientManager;
    SearchData GlobalSearchData;
	// Use this for initialization
	void Start () {
        Debug.Log("Searching for Matches");
        GameObject t = GameObject.Find("NetManager");
        if (t != null)
        {
            ClientManager = t.GetComponent<NetManager>();
        }
        StartCoroutine("GetServers");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator GetServers()
    {
        //If ClientManager was found, grab match making data.
        if (ClientManager != null)
        {
            string url = "";
            if (GlobalSearchData != null)
            {
                //Use search parameters,
                url = "http://www.jaytechmedia.com/autem/list.php";
            }
            else
            {
                url = "http://www.jaytechmedia.com/autem/list.php";
            }
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
        }
        yield return null;
    }

    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
