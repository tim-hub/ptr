using UnityEngine;
using System.Collections;

public class StartCanvas : MonoBehaviour {

	//public GameObject gameManager;
	// Use this for initialization
	void Awake () {
		Debug.Log(Application.loadedLevel);
		if(PlayerPrefs.GetInt(Application.loadedLevel.ToString("0"),0)!=0){ //!=0 it is not the first time to run
			gameObject.SetActive(false);

		}else{ //the first time to run

			PlayerPrefs.SetInt(Application.loadedLevel.ToString("0"),1);
			PlayerPrefs.Save();
			Invoke("StartGame",2f);

		}
	
	}

	void StartGame(){
		gameObject.SetActive(false);

	}
	

}
