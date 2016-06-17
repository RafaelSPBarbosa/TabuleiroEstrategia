using UnityEngine;
using System.Collections;

public class SoundVolumeFix : MonoBehaviour {

	void Start()
    {
        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return null;
        GameObject ConfigManager = GameObject.Find("ConfigManager");
        GetComponent<AudioSource>().volume = ConfigManager.GetComponent<AudioSource>().volume * 3;
    }
}
