using UnityEngine;
using System.Collections;

public class AimScript : MonoBehaviour {

	public Material matConquered;
	public GameObject aimEffect;




	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Ball")){

			if(matConquered){
				GetComponent<Renderer>().material=matConquered;
			}else{

				GetComponent<Renderer>().material.color=Color.green;
			}

			if(aimEffect){

				Instantiate(aimEffect,transform.position,Quaternion.identity);
			}

			Debug.Log("Red Was Conquered");

			GameManager.instance.RedConquered();

		}
		

		
	}
}
