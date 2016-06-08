using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip MenuMusic , GameMusic;

    public AudioSource AS;

    void Start()
    {
        if (PlayerPrefs.HasKey("VOLUME"))
        {
            float VOLUME = PlayerPrefs.GetFloat("VOLUME");
            AS.volume = VOLUME;
        }
        else {
            PlayerPrefs.SetFloat("VOLUME", 1);
            AS.volume = 0.3f;
        }
    }

	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (AS.clip != MenuMusic)
            {
                float VOLUME = PlayerPrefs.GetFloat("VOLUME");
                AS.volume = VOLUME;
                AS.clip = MenuMusic;
                AS.Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (AS.clip != GameMusic)
            {
                float VOLUME = PlayerPrefs.GetFloat("VOLUME");
                AS.volume = VOLUME * 0.3f;
                AS.clip = GameMusic;
                AS.Play();
            }
        }
	}

    public void UpdateVolume()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            float VOLUME = PlayerPrefs.GetFloat("VOLUME");
            AS.volume = VOLUME;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            float VOLUME = PlayerPrefs.GetFloat("VOLUME");
            AS.volume = VOLUME * 0.3f;
        }
    }
}
