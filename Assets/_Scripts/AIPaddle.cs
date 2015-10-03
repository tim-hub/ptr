using UnityEngine;
using System.Collections;

public class AIPaddle : MonoBehaviour {
	//use paddle as a manager to ball


	#region Properties
	public GameObject ball;
	public float paddleSpeed=7f;
	public float aiSmart=0f; //if it is 1, then the offset between 2 centers of paddle and ball will gone



	private float posX;
	private Vector3 originPos;
	private Vector3 StartPos;
	private float leftBorder;
	private float rightBorder;
	private bool paddleInitiate;

	private float randomTime;
	private float ballTiming=0f;

	private bool ballDown;


	private Vector3 oldBallPos;

	#endregion


	void Start () {

		leftBorder=GameManager.instance.leftBorder+transform.localScale.x/2f;
		rightBorder=GameManager.instance.rightBorder-transform.localScale.x/2f;


		paddleInitiate=true;  
		StartPos= GetNewPos();

		oldBallPos=ball.transform.position;

		randomTime=Random.Range(1f,3f);
	}


	void FixedUpdate(){
		if(paddleInitiate){

			PaddleStart ();

			BallRandomStart();

		}else{//do some AI logic work



			PaddleStrategy();
		}


	}



	#region AIStrategies

	void PaddleStrategy(){

		if(ball){
			//check ball move direction and ?halt
			Vector3 ballVelocity=(ball.transform.position-oldBallPos)/Time.deltaTime;
			//Debug.Log("ballVelocity"+ballVelocity);
			if(ballVelocity.y<0){
				ballDown=true;
			}else{
				ballDown=false;
			}

			oldBallPos=ball.transform.position;
			
			
			if(ballDown){
				//ball down
				if(MakeChoice(60)){//60 percents follow the ball position x
					PaddleMove (GetPaddleVector3(ball.transform.position.x));
					
				}
				//else do nothing, the paddle stay
				
			}else{ //ball up
				if(MakeChoice(90)){//90 percents
					float t=((transform.position-ball.transform.position).y-aiSmart)/ballVelocity.y;
					float aimX=ball.transform.position.x+t*ballVelocity.x;
					
					PaddleMove (GetPaddleVector3(aimX));
					
					
				}else if(MakeChoice(50)){//1 percent
					PaddleMove (GetPaddleVector3(ball.transform.position.x));
					
				}else{//1 percent
					
					//donothing stay now, maybe change to move randomly
				}
				
			}

		}

	}

	bool MakeChoice(int percent){
		int tmp=Random.Range(0,100);

		if (tmp<percent){

			return true;
		}else{
			return false;
		}

	}

	Vector3 GetPaddleVector3(float x){

		return new Vector3(x, transform.position.y,transform.position.z);

	}

	#endregion

	#region PaddleStart
	/// <summary>
	/// Paddles init start
	/// </summary>
	void PaddleStart(){


		if(transform.position==StartPos){
			StartPos=GetNewPos();
		}
		PaddleMove(StartPos);


	}

	/// <summary>
	/// Gets the new position. Randomdly from left border to right border
	/// </summary>
	/// <returns>The new position.</returns>
	Vector3 GetNewPos(){
		
		return new Vector3(Random.Range(leftBorder,rightBorder),transform.position.y,0);
		
	}
	#endregion

	#region PaddleMove


	/// <summary>
	/// Paddles start.smoothly
	/// </summary>
	void PaddleMove(Vector3 newPos){

		newPos= new Vector3(Mathf.Clamp(newPos.x,leftBorder,rightBorder),newPos.y,newPos.z);
		if(newPos.x<transform.position.x){
			PaddleLeft(newPos);
		}
		if(newPos.x>transform.position.x){
			PaddleRight(newPos);
		}

	}



	/// <summary>
	/// Paddles move to the left.
	/// </summary>
	/// <param name="newPos">New position.</param>
	void PaddleLeft(Vector3 newPos){
		//transform.position=Vector3.Lerp(originPos,newPos,paddleSpeed); //use lerp to move smoothly
		transform.Translate(new Vector3(-1,0,0)*paddleSpeed*Time.deltaTime);
		//GetComponent<Rigidbody>().MovePosition(newPos);
	}
	void PaddleRight(Vector3 newPos){
		//transform.position=Vector3.Lerp(originPos,newPos,paddleSpeed);
		transform.Translate(new Vector3(1,0,0)*paddleSpeed*Time.deltaTime);
		//GetComponent<Rigidbody>().MovePosition(newPos);
	}

	#endregion


	/// <summary>
	/// Balls the random start.
	/// </summary>

	void BallRandomStart(){
		ballTiming+=Time.deltaTime;
		if(ballTiming>randomTime){
			ball.SendMessage("SetUp",SendMessageOptions.DontRequireReceiver);
			Debug.Log ("Paddle send to ball set up");

			GameManager.instance.BallRun();

			paddleInitiate=false;

		}

	}

}
