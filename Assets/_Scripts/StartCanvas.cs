using UnityEngine;
using System.Collections;

public class StartCanvas : MonoBehaviour {
	public string nextLevel;

	//public GameObject gameManager;
	// Use this for initialization
	void Awake () {
		Debug.Log(Application.loadedLevel);
		if(PlayerPrefs.GetInt(Application.loadedLevel.ToString("0"),0)!=0){ //!=0 it is not the first time to run
			StartGame(); //then run game imediatelly

		}else{ //the first time to run

			PlayerPrefs.SetInt(Application.loadedLevel.ToString("0"),1); //set it, next time, it is not the first
			PlayerPrefs.Save();
			Invoke("StartGame",3.5f);

		}
	
	}

	void StartGame(){
		Application.LoadLevel(nextLevel);

	}
	

}
