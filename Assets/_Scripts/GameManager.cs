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
	public GameObject ball;

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

	private float runningTime=0f;
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

		paddle.transform.position=new Vector3(cameraPos.x,topBorder-0.5f,0f);
		Debug.Log("Set Padle Position");
	}


	void Start () {
		//move gamemanager to aim, to listen
		transform.position=aim.transform.position;

		score=0;
		highestScore=PlayerPrefs.GetInt("HighestScore",0);


		
		
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

		if(bricksArrayName=="Rectangle, Triangle or Wave" ||bricksArrayName=="Rectangle"){
			bricksNum=20;
			SetBricksRectangleArray();

		}else if(bricksArrayName=="Triangle"){


		}else{ 


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
			runningTime += Time.deltaTime ;

			timeText.text="TIME: "+(runningTime).ToString("0.0");
		}
	}
	
	
	public void RedConquered(){

		Lose ();
		losePanelText.text="Mission Failde! \n Red Was Conquered";

	}

	public void BrickBreak(){
		bricksNum--; //one break down, breaks num --
		if (bricksNum==0){
			Lose ();
			losePanelText.text="Mission Failde! \n Your Bricks Are Gone";
		}

	}
	public void BallHalt(){
		ballHalt=true;

		Win();
		winPanelText.text=string.Format
			("Hero! Ball is Halt, \n You win in {0} seconds\n with {1}  bricks. \nYour score is {2}. \nThe highest is {3}.",
			 runningTime.ToString("0.0"),bricksNum, score, highestScore);


	}
	public void BallOut(){
		Win();
		winPanelText.text=string.Format
			("Hero! Ball is Out, \n You win in {0} seconds\n with {1}  bricks. \nYour score is {2}. \nThe highest is {3}.",
			 runningTime.ToString("0.0"),bricksNum, score, highestScore);

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
			score+=20;
		}else{ //ball out
			score+=100;
		}
		score+= 100-(int)(runningTime)+bricksNum;
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


	#region UI
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
		}else{
			AudioListener.pause=true;
			soundSwitch.GetComponentInChildren<Text>().text="Sound On";
		}

	}

//	public void SoundOn(){
//		AudioListener.pause=false;
//
//		soundOn.enabled=false;
//		soundOff.enabled=true;
//	}

	#endregion
}
