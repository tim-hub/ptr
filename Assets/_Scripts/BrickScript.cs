using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;



public class BrickScript : MonoBehaviour
	,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
						
{
	public GameObject brickParticle;
	public float dragScale=1.2f;

	private Rigidbody rb;

	private Vector2 beginPos;
	private Vector2 endPos;

	private bool dragging=false;

	private Vector2 delta;

	private float leftBorder;
	private float rightBorder;
	private float upBorder;
	private float downBorder;


	void Start(){
		rb=GetComponent<Rigidbody>();

		leftBorder=GameManager.instance.leftBorder+transform.localScale.x/2f;
		rightBorder=GameManager.instance.rightBorder-transform.localScale.x/2f;
		upBorder=GameManager.instance.topBorder-transform.localScale.y/2f; 
		downBorder=GameManager.instance.downBorder+transform.localScale.y/2f;

	}


	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Ball")){

			if(brickParticle){
				Instantiate(brickParticle,transform.position,Quaternion.identity);
			}

			Destroy(gameObject);
			GameManager.instance.BrickBreak();
		}


//		if(other.gameObject.CompareTag("Brick")){  //this does not work
//			Debug.Log("A brick touch another brick");
//
//			Vector3 distance= transform.position-other.gameObject.transform.position;
//			transform.position=ClampPosition(other.gameObject.transform.position+distance);
//
//		}

	}

	Vector3 ClampPosition(Vector3 pos){

		return new Vector3(Mathf.Clamp(pos.x,leftBorder,rightBorder),Mathf.Clamp(pos.y,downBorder,upBorder)
		                   ,pos.z);
	}



	void Update(){


	}



	public void OnBeginDrag(PointerEventData data){

		Debug.Log ("Drag Begin ");
		transform.localScale*=dragScale;

		//change the material
	}

	public void OnDrag(PointerEventData data)
	{
		//Debug.Log(downBorder+"to"+upBorder);
		//Debug.Log("Drag delta"+data.delta);


		//use this way, screen to view and view to world to consider the z of camera to support both orthographic and perspective
		//Debug.Log("Screen "+data.position);
		Vector3 viewPos=Camera.main.ScreenToViewportPoint(data.position);
		//Debug.Log("View "+viewPos);
		Vector3 pos =Camera.main.ViewportToWorldPoint(new Vector3(viewPos.x,viewPos.y,Mathf.Abs(Camera.main.transform.position.z)));
		//Debug.Log("World "+pos);


//		pos.z=0f;
//		rb.MovePosition(ClampPosition(pos));
		transform.position=ClampPosition(pos);



	}

	public void OnEndDrag(PointerEventData data){
		Debug.Log ("End Drag");



	}
	public void OnDrop(PointerEventData data){
		Debug.Log ("On Drog");
		//change the material
		transform.localScale/=dragScale;

	}

}
