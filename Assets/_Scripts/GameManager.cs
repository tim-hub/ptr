using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance=null;

	#region properties
	public GameObject aim;
	public GameObject environment;
	public GameObject wallCube;
	public GameObject sky; //to receive the ball
	public GameObject paddle;
	public GameObject paddle2;
	//public GameObject ball;

	public GameObject brick; //Birck Prefab
	public Transform bricksParent;
	public string bricksArrayName="Rectangle, Triangle or Wave";

	public GameObject winCanvas; //ui
	public GameObject loseCanvas;
	public GameObject mainCanvas;
	public GameObject pauseMenu;
	public Text timeText; //time text
	public Text winPanelText;
	public Text losePanelText;
	public Button soundSwitch;

	public float remainingTime=100f;

	public string nextLevel;


	//4 borders
	[HideInInspector]public float leftBorder;
	[HideInInspector]public float rightBorder;
	[HideInInspector]public float topBorder;
	[HideInInspector]public float downBorder;
	
	private Vector3 cameraPos;



	private float width;
	private float height;

	//bricks
	private List<GameObject> bricks=new List<GameObject>();
	private int bricksNum;
	private float brickInterval=3f; //the interval between bricks

	//some bools
	private bool ballHalt;
	private bool ballRunning;


	private int score=0;
	private int highestScore=0;

	#endregion

	// Use this for initialization
	void Awake(){

		if(instance==null){
			instance=this;
		}else if(instance!=this){
			Destroy(gameObject); //than destroy this
		}
			

		SetBasicValues();
		SetEnvironment();
		SetAimPosition();
		SetPaddlePosition();


	}

	void SetBasicValues(){
		cameraPos=Camera.main.transform.position;

		//the up right corner
		Vector3 cornerPos=Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,
		                                                               Mathf.Abs(-Camera.main.transform.position.z)));
		
		leftBorder=Camera.main.transform.position.x-(cornerPos.x-Camera.main.transform.position.x);
		rightBorder=cornerPos.x;
		topBorder=cornerPos.y;
		downBorder=Camera.main.transform.position.y-(cornerPos.y-Camera.main.transform.position.y);

		width=rightBorder-leftBorder;
		height=topBorder-downBorder;

	}

	void SetEnvironment(){
		//Walls
		GameObject wallHorizontal=Instantiate(wallCube,new Vector3(leftBorder-0.5f,cameraPos.y,0),Quaternion.identity)as GameObject;
		wallHorizontal.transform.parent=environment.transform;
		wallHorizontal.transform.localScale=new Vector3(1,height,1);

		(Instantiate(wallHorizontal,new Vector3(rightBorder+0.5f,cameraPos.y,0),Quaternion.identity)as GameObject).transform.parent=environment.transform;


		GameObject wallVertical=Instantiate(wallCube,new Vector3(cameraPos.x,downBorder-0.5f,0),Quaternion.identity)as GameObject;
		wallVertical.transform.parent=environment.transform;
		wallVertical.transform.localScale=new Vector3(width,1,1);

		//Sea
		GameObject skyPlane=(Instantiate(sky,new Vector3(cameraPos.x,topBorder+0.5f,0),Quaternion.identity)as GameObject);
		skyPlane.transform.parent=environment.transform;
		skyPlane.transform.localScale=new Vector3(width,1f,0.3f);
		
	}

	void SetAimPosition(){


		aim.transform.position=new Vector3(0,downBorder-0.6f,0);

	}

	void SetPaddlePosition(){

		paddle.transform.position=new Vector3(cameraPos.x-2,topBorder-0.5f,0f);
		Debug.Log("Set Padle Position");

		if(paddle2){
			paddle2.transform.position=new Vector3(cameraPos.x+2,topBorder-0.5f,0f);
			Debug.Log("Set Padle2 Position");
		}
	}


	void Start () {
		//move gamemanager to aim, to listen
		transform.position=aim.transform.position;

		score=0;
		highestScore=PlayerPrefs.GetInt("HighestScore",0);

		Time.timeScale=1.0f;
		
		
		//set interval of bricks
		if (rightBorder-leftBorder>=22){
			brickInterval=4;
		}else{
			brickInterval=3;
		}

		//set ui
		if(winCanvas){
			winCanvas.SetActive(false);
		}
		if(loseCanvas){
			loseCanvas.SetActive(false);
		}
		if(mainCanvas){
			mainCanvas.SetActive(true);
		}


		if(bricksArrayName=="Rectangle, Triangle or Wave" ||bricksArrayName=="Rectangle"){
			bricksNum=20;
			SetBricksRectangleArray();

		}else if(bricksArrayName=="Triangle"){


		}else{ 


		}

		if(PlayerPrefs.GetInt("Mute",0)==1){ //sound mute
			AudioListener.pause=true;
			soundSwitch.GetComponentInChildren<Text>().text="Sound On";
		}else{
			AudioListener.pause=false;
			soundSwitch.GetComponentInChildren<Text>().text="Sound Off";
		}



	}


	void SetBricksRectangleArray(){
		for (int i=0;i<bricksNum;i++){
			Vector3 pos=new Vector3(-1.5f*brickInterval+(i%4)*brickInterval,1f-(i/4)*2f);
			GameObject newBrick=Instantiate(brick,pos,Quaternion.identity) as GameObject;
			newBrick.transform.parent=bricksParent;
			bricks.Add(newBrick);

		}

	}


	// Update is called once per frame
	void Update () {
		if(ballRunning){ //if ball start, stat to count time
			remainingTime -= Time.deltaTime ;

			timeText.text="TIME: "+(remainingTime).ToString("0.0");

			if(remainingTime<=0){ //time out, then win
				remainingTime=0;
				ballRunning=false;
				EnermyTimeOut();
			}
		}

	}
	
	
	public void RedConquered(){

		Lose ();
		losePanelText.text=string.Format("Red Was Conquered."
										);

	}

	public void BrickBreak(){
		bricksNum--; //one break down, breaks num --
		if (bricksNum==0){
			Lose ();
			losePanelText.text=string.Format("Your Bricks Are Gone."
			                                 );
		}

	}
	public void BallHalt(){
		ballHalt=true;

		Win();
		winPanelText.text=string.Format
			("Hero! Ball is Halt, \n You still have {0} seconds\nwith {1}  bricks. \nYour score is {2}. \nYour highest is {3}.",
			 remainingTime.ToString("0.0"),bricksNum, score, highestScore);


	}
	public void BallOut(){
		Win();
		winPanelText.text=string.Format
			("Hero! Ball is Out, \n You still have {0} seconds\n with {1}  bricks. \nYour score is {2}. \nYour highest is {3}.",
			 remainingTime.ToString("0.0"),bricksNum, score, highestScore);

	}
	public void EnermyTimeOut(){
		Win();
		winPanelText.text=string.Format
			("Hero! Colds Time Out, \n You still have {0} seconds\n with {1}  bricks. \nYour score is {2}. \nYour highest is {3}.",
			 remainingTime.ToString("0.0"),bricksNum, score, highestScore);

	}
	public void Win(){
		//GetComponents<AudioSource>()[0].Pause();
		GetComponents<AudioSource>()[1].Play();
		score=GetGameScore();
		SetHighestScore();
		GameOver();
		if(winCanvas){
			winCanvas.SetActive(true);
		}


	}
	public void Lose(){
		GetComponents<AudioSource>()[2].Play(); //the third one, game over effect
		GameOver();
		if(loseCanvas){
			loseCanvas.SetActive(true);
		}

	}

	private int GetGameScore(){ //calculate win score  win!!!
		int score=0;
		if(ballHalt){
			score+=100;
		}else{ //ball out
			score+=20;
		}
		score+= (int)(remainingTime)+bricksNum;
		return score;

	}
	private void SetHighestScore(){
		if(score>highestScore){
			highestScore=score;
			PlayerPrefs.SetInt("HighestScore",score);
			PlayerPrefs.Save();

		}

	}

	public void GameOver(){


		Time.timeScale=0.25f;

		mainCanvas.SetActive(false);
		ballRunning=false;
		Invoke("Stop",1f);
	}

	public void Stop(){
		Time.timeScale=0f;
	}

	public void BallRun(){
		ballRunning=true;
	}


	public void Pause(){
		Time.timeScale=0f;
		GetComponent<AudioSource>().Pause();
		pauseMenu.SetActive(true);
		// about the sound swithc button
		if(AudioListener.pause==true){
			
			soundSwitch.GetComponentInChildren<Text>().text="Sound On";
		}else{
			
			soundSwitch.GetComponentInChildren<Text>().text="Sound Off";
		}
		mainCanvas.SetActive(false);
	}
	public void RauseResume(){
		Time.timeScale=1f;
		GetComponent<AudioSource>().Play();
		pauseMenu.SetActive(false);
		mainCanvas.SetActive(true);
	}
	
	
	public void PlayAgain(){
		
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale=1f;
	}
	public void Restart(){
		Application.LoadLevel("Normal");
		Time.timeScale=1f;
	}
	public void NextLevel(){
		Application.LoadLevel(nextLevel);
		Time.timeScale=1f;
		winCanvas.SetActive(false);
	}
	
	public void ExitLevelToMenu(){
		
		Application.LoadLevel("Menu");
	}
	
	
	public void ExitGame(){
		#if UNITY_EDITOR
		
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		
		Application.Quit();
		#endif
	}
	
	public void Help(){
		
		Application.OpenURL("https://tim.bai.uno/ptr.htm");
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

}
