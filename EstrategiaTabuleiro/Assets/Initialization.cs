using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Initialization : MonoBehaviour {

    public GameObject MusicManager;

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(MusicManager);
        StartCoroutine("LoadMenu");
	}

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
    }
}
