using UnityEngine;
using System.Collections;

public class AimScript : MonoBehaviour {

	public Material matConquered;
	public GameObject aimEffect;

	private bool conquered=false; //use this to avoid 2 ball's 2 collision in 0.25 time scale



	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Ball") &&(!conquered)){

			if(matConquered){
				GetComponent<Renderer>().material=matConquered;
			}else{

				GetComponent<Renderer>().material.color=Color.green;
			}

			if(aimEffect){

				Instantiate(aimEffect,transform.position,Quaternion.identity);
			}

			Debug.Log("Red Was Conquered");
			conquered=true;
			GameManager.instance.RedConquered();

		}
		

		
	}
}
