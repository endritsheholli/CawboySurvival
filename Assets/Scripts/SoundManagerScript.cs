using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {
	
	public static AudioClip getPointSound, loseLifeSound;
	static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
		getPointSound = Resources.Load<AudioClip> ("getPoint");
		loseLifeSound = Resources.Load<AudioClip> ("loseLife");
		
		audioSrc = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static void PlaySound(string clip)
	{
		switch(clip)
		{
			case "getPoint":
				audioSrc.PlayOneShot(getPointSound);
				break;
			case "loseLife":
				audioSrc.PlayOneShot(loseLifeSound);
				break;
		}
	}
}
