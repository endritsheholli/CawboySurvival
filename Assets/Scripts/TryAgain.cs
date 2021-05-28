using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TryAgain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	public void TryAgainOnClick()
	{
		Application.LoadLevel(1);
	}
	public void ExitOnCilc()
	{
		Application.Quit();
	}
	public void MainSceneOnClick()
	{
		Application.LoadLevel(2);
	}
	public void InstructionsOnClick()
	{
		Application.LoadLevel(3);
	}
}
