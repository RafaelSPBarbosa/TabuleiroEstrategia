using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip MenuMusic , GameMusic;

    public AudioSource AS;

	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (AS.clip != MenuMusic)
            {
                AS.clip = MenuMusic;
                AS.Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (AS.clip != GameMusic)
            {
                AS.clip = GameMusic;
                AS.Play();
            }
        }
	}
}
