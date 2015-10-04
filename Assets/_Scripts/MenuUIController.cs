using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuUIController : MonoBehaviour {

	public Text scoreText;
	public GameObject startBut;
	public GameObject levelChoose;
	public Button soundSwitch;

	public GameObject menuCanvas;
	public GameObject howToCanvas;
	public GameObject creditsCanvas;

	public GameObject firstStartCanvas;

	void Awake(){
		if(PlayerPrefs.GetInt("Previously",0)==0){
			menuCanvas.SetActive(false);
			firstStartCanvas.SetActive(true);


		}else{
			firstStartCanvas.SetActive(false);
			menuCanvas.SetActive(true);
		}



	}



	// Use this for initialization
	void Start () {
		scoreText.text=string.Format("Score: {0}",PlayerPrefs.GetInt("HighestScore",0));


		if(PlayerPrefs.GetInt("Mute",0)==1){ //sound mute
			AudioListener.pause=true;
			soundSwitch.GetComponentInChildren<Text>().text="Sound On";
		}else{
			AudioListener.pause=false;
			soundSwitch.GetComponentInChildren<Text>().text="Sound Off";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void LevelChoose(){
		startBut.SetActive(false);
		levelChoose.SetActive(true);
	
	}

	public void StartLevel1(){

		Application.LoadLevel("NormalSplash");
	}

	public void StartLevel2(){
		
		Application.LoadLevel("CrazySplash");
	}

	public void SoundSwitch(){

		if(AudioListener.pause==true){
			AudioListener.pause=false;
			soundSwitch.GetComponentInChildren<Text>().text="Sound Off";
			PlayerPrefs.SetInt("Mute",0);
		}else{
			AudioListener.pause=true;
			soundSwitch.GetComponentInChildren<Text>().text="Sound On";
			PlayerPrefs.SetInt("Mute",1);
		}
		PlayerPrefs.Save();
	}

	public void HowTo(){
		menuCanvas.SetActive(false);
		howToCanvas.SetActive(true);
	}

	public void More(){

		Application.OpenURL("http://geekgame.bai.uno/ptr");
	}

	public void Continue(){
		menuCanvas.SetActive(true);
		howToCanvas.SetActive(false);
		creditsCanvas.SetActive(false);
	}



	public void CreditsBut(){
		menuCanvas.SetActive(false);
		creditsCanvas.SetActive(true);
	}

	public void ExitGame(){
		#if UNITY_EDITOR
		
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		
		Application.Quit();
		#endif
	}

	public void PreviousContinue(){
		firstStartCanvas.SetActive(false);
		PlayerPrefs.SetInt("Previously",1);
		PlayerPrefs.Save();
		menuCanvas.SetActive(true);
	}
	
}
