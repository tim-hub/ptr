using UnityEngine;
using System.Collections;

public class BallCollisionEffect : MonoBehaviour {

	public GameObject collisionEffect;

	void OnCollisionEnter(Collision other){
		if(other.gameObject.CompareTag("Ball")){
			if(collisionEffect){
				Instantiate(collisionEffect,other.gameObject.transform.position,Quaternion.identity);
			}
		}
	}
}
