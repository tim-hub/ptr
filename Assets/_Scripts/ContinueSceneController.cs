using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueSceneController : MonoBehaviour {

	public void MoreBut(){
		Application.OpenURL("https://tim.bai.uno/ptr");
	}

	public void Exit(){
		Application.LoadLevel("Menu");
	}


}
