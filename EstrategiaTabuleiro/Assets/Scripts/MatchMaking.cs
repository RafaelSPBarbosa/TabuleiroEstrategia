using UnityEngine;
using System.Collections;

public class MatchMaking : MonoBehaviour {

    public string www_data = "";

	// Use this for initialization
	void Start () {
        Debug.Log("Searching for Matches");
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
        Debug.Log(www.text);
        //Convert JSON into unity useable array
       // JsonUtility.FromJson(www.text, MatchMakingData);
        yield return null;
    }
}
