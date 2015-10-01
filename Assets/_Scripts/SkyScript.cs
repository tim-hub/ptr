using UnityEngine;
using System.Collections;

public class SkyScript : MonoBehaviour {

	public GameObject ballEffect;

	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag("Ball")){
			if(ballEffect){
				Instantiate(ballEffect,other.gameObject.transform.position,Quaternion.identity);
			}
			Destroy(other.gameObject);

			Debug.Log("Ball Out");
			
			GameManager.instance.BallOut();
			
		}
		
		
		
	}
}
