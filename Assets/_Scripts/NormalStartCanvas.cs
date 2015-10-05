using UnityEngine;
using System.Collections;

public class NormalStartCanvas : MonoBehaviour {

	public string nextLevel;

	//public GameObject gameManager;
	// Use this for initialization
	void Awake () {
		Invoke("StartGame",3.5f);
	}

	void StartGame(){


		Application.LoadLevel(nextLevel);
	}
	

}
