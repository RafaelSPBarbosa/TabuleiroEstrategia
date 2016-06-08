using UnityEngine;
using System.Collections;

public class UnitSoundFX : MonoBehaviour {

    AudioSource As;

    void Start()
    {
        As = GetComponent<AudioSource>();
    }

	public void AttackSFX(AudioClip clip)
    {
        As.clip = clip;
        As.Play();
    }
}
