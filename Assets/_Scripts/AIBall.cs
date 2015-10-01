using UnityEngine;
using System.Collections;

public class AIBall : MonoBehaviour {

	public float ballInitialVelocity=600f;
	public float ballMinVelocity=1f;

	private bool ballInPlay;
	private bool ballsetUp;
	private Rigidbody rb;	
	private float startAngle;
	private Vector3 oldPosition;
	private bool ballRunning=false;


	void Start(){
		startAngle=Random.Range(45f,135f);
		rb=GetComponent<Rigidbody>();
		oldPosition=transform.position;
	}

	void FixedUpdate () {
		if(ballsetUp){ 
			Debug.Log ("Click and AI Ball Set UP");

			BallStart();
			ballsetUp=false;


		}


		if(ballRunning){ //if ball run,check the ball is halt or not

			transform.position=new Vector3(transform.position.x,transform.position.y,0f); //add strict to position

			Vector3 ballVelocity=(transform.position-oldPosition)/Time.deltaTime;

			if(Mathf.Abs(ballVelocity.x)<ballMinVelocity&&
			   		(Mathf.Abs(ballVelocity.y)<ballMinVelocity)){

				GameManager.instance.BallHalt(); //if ball halt player win

			}
			oldPosition=transform.position;

		}
		
	}

	public void BallStart(){
		//iniate ball
		transform.parent=null;

		rb.isKinematic=false;

		rb.AddForce(ballInitialVelocity*(new Vector3(Mathf.Cos(180-startAngle),
		                            -Mathf.Abs(Mathf.Sin(180-startAngle)),0))); //make the direction is up

		Invoke("SetBallRunning",0.1f);
	
	}

	void SetBallRunning(){
		ballRunning=true;
	}

	public void SetUp(){
		Debug.Log("Ball Received the message and set up");
		ballsetUp=true;
	}
	
}
